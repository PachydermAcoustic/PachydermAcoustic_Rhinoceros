//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL) by Arthur van der Harten 
//' 
//'This file is part of Pachyderm-Acoustic. 
//' 
//'Copyright (c) 2008-2015, Arthur van der Harten 
//'Pachyderm-Acoustic is free software; you can redistribute it and/or modify 
//'it under the terms of the GNU General Public License as published 
//'by the Free Software Foundation; either version 3 of the License, or 
//'(at your option) any later version. 
//'Pachyderm-Acoustic is distributed in the hope that it will be useful, 
//'but WITHOUT ANY WARRANTY; without even the implied warranty of 
//'MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the 
//'GNU General Public License for more details. 
//' 
//'You should have received a copy of the GNU General Public 
//'License along with Pachyderm-Acoustic; if not, write to the Free Software 
//'Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA. 

using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace Pachyderm_Acoustic
{
    namespace Audio
    {
        public partial class Pach_SP
        {
            public class IIR_Design
            {
                //public static Complex[] SpectrumFromIIR(Complex[] a, Complex[] b, int fs = 44100, int n = 512)
                //{
                //    double[] ad = new double[a.Length], bd = new double[b.Length];
                //    for (int i = 0; i < a.Length; i++) ad[i] = a[i].Magnitude;
                //    for (int i = 0; i < b.Length; i++) bd[i] = b[i].Magnitude;

                //    return SpectrumFromIIR(ad, bd, fs, n);
                //}

                //public static Complex[] SpectrumFromIIR(double[] a, double[] b, int fs = 44100, int n = 512)
                //{
                //    int k = Math.Max(b.Length, a.Length);

                //    double[] f = new double[n];
                //    for (int i = 0; i < n; i++) f[i] = (double)fs * i / (2 * n);

                //    int pad_sz = n * (int)Math.Ceiling((double)k / n);
                //    Array.Resize(ref b, 2 * n);
                //    Array.Resize(ref a, 2 * n);

                //    Complex[] h = new Complex[n];

                //    Complex[] pb = FFT_General(b, 0);
                //    Complex[] pa = FFT_General(a, 0);

                //    for (int i = 0; i < h.Length; i++) h[i] = pb[i] / pa[i];
                //    return h;
                //}

                public static Complex[] AutoCorrelation_Coef(Complex[] X, int maxlag)
                {
                    Complex[] r_l = new Complex[maxlag];

                    for (int lag = 1; lag <= maxlag; lag++)
                    {
                        Complex mean = 0;
                        for (int i = 0; i < X.Length; i++) mean += X[i];
                        mean /= X.Length;
                        Complex denom = 0;
                        Complex num = 0;

                        int N_k = X.Length - lag;

                        for (int i = 0; i < N_k; i++)
                        {
                            Complex x_X = X[i] - mean;
                            denom += x_X * x_X;
                            num += x_X * Complex.Conjugate(X[i + lag] - mean);
                        }

                        for (int i = 0; i < lag; i++)
                        {
                            int idx = N_k + i;
                            Complex x_X = X[idx] - mean;
                            denom += x_X * x_X;
                        }

                        r_l[lag - 1] = num / denom;
                    }

                    return r_l;
                }

                public static Complex[] RLC_FreqResponse(List<double> R, List<double> L, List<double> C, double[] frequencies)
                {
                    Complex[] response = new Complex[frequencies.Length];

                    if (R.Count != L.Count && L.Count != C.Count) throw new Exception("Poles and Zeros must be arranged in pairs...");

                    for (int f = 0; f < frequencies.Length; f++)
                    {
                        Complex w = Complex.ImaginaryOne * Utilities.Numerics.PiX2 * frequencies[f];
                        response[f] = 1;
                        for (int i = 0; i < R.Count; i++) response[f] *= L[i] * w + R[i] + C[i] / w;
                    }
                    return response;
                }

                public static Complex[] PZ_FreqResponse(List<Complex> poles, List<Complex> zeros, double gain, double[] frequencies)
                {
                    Complex[] response = new Complex[frequencies.Length];

                    for (int f = 0; f < frequencies.Length; f++)
                    {
                        Complex s = Complex.ImaginaryOne * Utilities.Numerics.PiX2 * frequencies[f];
                        response[f] = gain;
                        for (int i = 0; i < poles.Count; i++) response[f] /= (s - poles[i]);
                        for (int i = 0; i < zeros.Count; i++) response[f] *= (s - zeros[i]);
                    }

                    return response;
                }

                public static Complex[] AB_FreqResponse(List<Complex> b, List<Complex> a, double[] frequencies)
                {
                    Complex[] response = new Complex[frequencies.Length];

                    for (int f = 0; f < frequencies.Length; f++)
                    {
                        Complex s = Complex.ImaginaryOne * Utilities.Numerics.PiX2 * frequencies[f];
                        Complex num = 0, den = 0;
                        for (int i = 0; i < b.Count(); i++) num += b[i] * Complex.Pow(s, b.Count - i - 1);
                        for (int i = 0; i < a.Count(); i++) den += a[i] * Complex.Pow(s, a.Count - i - 1);
                        response[f] = num / den;
                    }

                    return response;
                }

                public static Complex[] AB_FreqResponse(List<double> b, List<double> a, double[] frequencies)
                {
                    Complex[] response = new Complex[frequencies.Length];

                    for (int f = 0; f < frequencies.Length; f++)
                    {
                        Complex s = Complex.ImaginaryOne * Utilities.Numerics.PiX2 * frequencies[f];
                        Complex num = 0, den = 0;
                        for (int i = 0; i < b.Count(); i++) num += b[i] * Complex.Pow(s, b.Count - i - 1);
                        for (int i = 0; i < a.Count(); i++) den += a[i] * Complex.Pow(s, a.Count - i - 1);
                        response[f] = num / den;
                    }

                    return response;
                }

                public class IIR_Fitting_Spectrum_Objective : LibOptimization.Optimization.absObjectiveFunction
                {
                    int fs;
                    int n;
                    //int[] sampled;
                    Complex[] Spectrum;
                    Complex[] sel_Spectrum;
                    Complex[] abs;
                    double[] freq;
                    double[] sel_freq;
                    int p_conj;

                    public IIR_Fitting_Spectrum_Objective(int order, Complex[] BasisSpectrum, int samplingFrequency)
                    {
                        //Find all local maxima (within 10 points)
                        Spectrum = BasisSpectrum;
                        fs = samplingFrequency;
                        n = order;
                        p_conj = (int)(2 * Math.Floor((double)n / 2));
                        for (int i = 0; i < freq.Length; i++) freq[i] = (double)(i * samplingFrequency) / freq.Length;
                    }

                    public override double F(List<double> x)
                    {
                        throw new NotImplementedException();
                        Complex[] p = new Complex[n];
                        int p_conj = (int)(2 * Math.Floor((double)n / 2));
                        for (int i = 0; i < p_conj; i++)
                        {
                            p[2 * i] = Complex.FromPolarCoordinates(x[2 * i], x[2 * i + 1] * Math.PI * 2);
                            p[2 * i + 1] = Complex.Conjugate(p[i]);
                        }
                        for (int i = 0; i < n - p_conj; i++)
                        {
                            p[i + p_conj] = x[p_conj + i];
                        }
                        Complex[] a;
                        P2poly(p, out a
                            );
                        Complex[] Spectrum_Mod = new Complex[Spectrum.Length];
                        for (int i = 0; i < Spectrum.Length; i++)
                        {
                            Complex denom = 0;
                            Complex s = Complex.ImaginaryOne * freq[i] * Utilities.Numerics.PiX2;
                            Complex Spow = s;

                            for (int j = n-1; j >= 0; i++)
                            {
                                denom += a[j] * Spow;
                                Spow *= s;
                            }
                            Spectrum_Mod[i] = Spectrum[i] * denom;
                        }

                        //TODO: find a polynomial fit operation that works on complex numbers...
                        //MathNet.Numerics.Fit.Polynomial(freq, Spectrum_Mod, 3);
                    }

                    public override List<double> Gradient(List<double> x)
                    {
                        throw new NotImplementedException();
                    }

                    public override List<List<double>> Hessian(List<double> x)
                    {
                        throw new NotImplementedException();
                    }

                    public override int NumberOfVariable
                    {
                        get
                        {
                            return n + p_conj;
                        }
                    }
                }

//                public static void polynomial_Fit(Complex[] x, Complex[] y, int n)
//                { 

//  if (nargout > 2)
//## Normalized the x values.
//                        mu = [mean(x), std(x)];
//                    x = (x - mu(1)) / mu(2);
//                    endif

//  if (!size_equal(x, y))
//                        error("polyfit: X and Y must be vectors of the same size");
//                    endif

//  if (islogical(n))
//                        polymask = n;
//## n is the polynomial degree as given the polymask size; m is the
//## effective number of used coefficients.
//                    n = length(polymask) - 1; m = sum(polymask) - 1;
//  else
//    if (!(isscalar(n) && n >= 0 && !isinf(n) && n == fix(n)))
//                        error("polyfit: N must be a non-negative integer");
//                    endif
//                    polymask = logical(ones(1, n + 1)); m = n;
//                    endif

//                    y_is_row_vector = (rows(y) == 1);

//## Reshape x & y into column vectors.
//                    l = numel(x);
//                    x = x(:);
//                    y = y(:);

//## Construct the Vandermonde matrix.
//                    v = vander(x, n + 1);

//  ## Solve by QR decomposition.
//                [q, r, k] = qr(v(:, polymask), 0);
//  p = r \ (q' * y);
//  p(k) = p;
  
//  if (n != m)
//    q = p; p = zeros(n+1, 1);
//    p(polymask) = q;
//  endif
  
//  if (nargout > 1)
//    yf = v* p;

//    if (y_is_row_vector)
//      s.yf = yf.';
//    else
//      s.yf = yf;
//    endif
//    s.X = v; 

//    ## r.'*r is positive definite if X(:, polymask) is of full rank.
//    ## Invert it by cholinv to avoid taking the square root of squared
//    ## quantities. If cholinv fails, then X(:, polymask) is rank
//    ## deficient and not invertible.
//    try
//      C = cholinv(r.'*r)(k, k);
//    catch
//      C = NaN(m + 1, m + 1);
//                end_try_catch

//    if (n != m)
//      ## fill matrices if required
//      s.X(:, !polymask) = 0;
//      s.R = zeros(n+1, n+1); s.R(polymask, polymask) = r;
//      s.C = zeros(n+1, n+1); s.C(polymask, polymask) = C;
//    else
//      s.R = r; 
//      s.C = C;
//    endif
//    s.df = l - m - 1;
//                s.normr = norm(yf - y);
//                endif

//## Return a row vector.
//  p = p.';

//endfunction

//            }

            public class IIR_fd_Spectrum_Objective : LibOptimization.Optimization.absObjectiveFunction
                {
                    int fs;
                    int n;
                    int[] sampled;
                    Complex[] Spectrum;
                    Complex[] sel_Spectrum;
                    Complex[] abs;
                    double[] freq;
                    double[] sel_freq;
                    public double[] pole_freqs;

                    public IIR_fd_Spectrum_Objective(Complex[] BasisSpectrum, int samplingFrequency)
                    {
                        //Find all local maxima (within 10 points)
                        List<int> maxima = new List<int>();
                        freq = new double[BasisSpectrum.Length];

                        fs = samplingFrequency;
                        
                        for (int i = 0; i < freq.Length; i++) freq[i] = (double)(i * samplingFrequency) / freq.Length;

                        Spectrum = BasisSpectrum;
                        n = BasisSpectrum.Length;
                        fs = samplingFrequency;
                        sampled = new int[24];
                        sel_Spectrum = new Complex[24];
                        sel_freq = new double[24];
                        abs = new Complex[24];

                        for (int i = 0; i < 24; i++)
                        {
                            double f = 24.803 * Math.Pow(2, (double)i / 3);
                            sampled[i] = (int)(f * 2 * BasisSpectrum.Length / samplingFrequency);
                            sel_Spectrum[i] = Spectrum[sampled[i]];
                            abs[i] = AbsorptionModels.Operations.Absorption_Coef(sel_Spectrum[i]);
                            sel_freq[i] = freq[sampled[i]];
                        }

                        double[] alpha = AbsorptionModels.Operations.Absorption_Coef(BasisSpectrum);
                        double[] Derivative_Spectrum = new double[BasisSpectrum.Length - 1];
                        for (int i = 0; i < Derivative_Spectrum.Length; i++) Derivative_Spectrum[i] = alpha[i + 1] - alpha[i];

                        //for (int i = 10; i < Derivative_Spectrum.Length - 10; i++)
                        //{
                        //    bool max = true;
                        //    for (int s = -10; s < 10; s++)
                        //    {
                        //        if (Derivative_Spectrum[i] < Derivative_Spectrum[i + s])
                        //        {
                        //            max = false;
                        //            continue;
                        //        }
                        //    }
                        //    if (max) maxima.Add(i);
                        //}

                        //Check for a zero pole...
                        if (alpha[0] - alpha[3] > 0)
                        {
                            maxima.Add(0);
                        }

                        //Check for any other poles...
                        for (int i = 3; i < BasisSpectrum.Length - 3; i++)
                        {
                            bool max = true;
                            for (int s = -3; s < 3; s++)
                            {
                                if (alpha[i] > alpha[i + s])
                                {
                                    max = false;
                                    continue;
                                }
                            }
                            if (max) maxima.Add(i);
                        }

                        pole_freqs = new double[maxima.Count()];
                        for (int i = 0; i < pole_freqs.Length; i++) pole_freqs[i] = Utilities.Numerics.PiX2 * (double)(maxima[i] * samplingFrequency) / BasisSpectrum.Length;
                    }

                    public override double F(List<double> x)
                    {
                        double[] a, b;
                        DW2AB(x.ToArray(), pole_freqs, out a, out b);
                        Complex[] spec = AB_FreqResponse(new List<double>(b), new List<double>(a), sel_freq);

                        Complex[] alpha = new Complex[spec.Length];
                        double[] alpha_d = new double[spec.Length];

                        for (int i = 0; i < spec.Length; i++)
                        {
                            alpha[i] = AbsorptionModels.Operations.Absorption_Coef(spec[i]);
                            alpha_d[i] = alpha[i].Real;
                            if (alpha[i].Real > 1 || alpha[i].Real < 0) return double.PositiveInfinity;
                        }

                        //double ans = -Math.Abs(CrossCorrelation_Coef(spec, sel_Spectrum).Real);

                        double ans = CompareFunctions(abs, alpha, sampled);
                        //if (ans > -0.9) return 1000;
                        return ans;
                    }

                    public override List<double> Gradient(List<double> x)
                    {
                        throw new NotImplementedException();
                    }

                    public override List<List<double>> Hessian(List<double> x)
                    {
                        throw new NotImplementedException();
                    }

                    public override int NumberOfVariable
                    {
                        get
                        {
                            return pole_freqs.Length;
                        }
                    }
                }

                public class IIR_Spectrum_Objective:LibOptimization.Optimization.absObjectiveFunction
                {
                    int fs;
                    int n;
                    int[] sampled;
                    Complex[] Spectrum;
                    Complex[] sel_Spectrum;
                    Complex[] abs;
                    public int pzct;
                    int pno_conj;
                    int pno_conj_phase;
                    int pno_real;
                    int zno_real;
                    double[] freq;
                    double[] sel_freq;
                    public double[] pole_phase;

                    public IIR_Spectrum_Objective(int Filter_Order, Complex[] BasisSpectrum, int samplingFrequency)
                    {
                        //Find all local maxima (within 10 points)
                        List<int> maxima = new List<int>();
                        freq = new double[BasisSpectrum.Length];
                        for (int i = 10; i < BasisSpectrum.Length-10; i++)
                        {
                            bool max = true;
                            for (int s = -10; s < 10; s++)
                            {
                                if (BasisSpectrum[i].Real < BasisSpectrum[i + s].Real)
                                {
                                    max = false;
                                    continue;
                                }
                            }
                            if (max) maxima.Add(i);
                        }

                        fs = samplingFrequency;

                        for (int i = 0; i < freq.Length; i++) freq[i] = (double)(i * samplingFrequency) / freq.Length;

                        pole_phase = new double[maxima.Count];
                        for (int i = 0; i < maxima.Count; i++) pole_phase[i] = 2 * Math.PI * maxima[i] / BasisSpectrum.Count();

                        if (Filter_Order < maxima.Count * 2) Filter_Order = maxima.Count * 2;

                        pzct = Filter_Order;
                        pno_conj = (int)Math.Floor((double)pzct / 2);
                        pno_conj_phase = (int)Math.Floor((double)pzct / 2 - maxima.Count());
                        pno_real = pzct % 2;
                        zno_real = pzct;
                        Spectrum = BasisSpectrum;
                        n = BasisSpectrum.Length;
                        fs = samplingFrequency;
                        sampled = new int[24];
                        sel_Spectrum = new Complex[24];
                        sel_freq = new double[24];
                        abs = new Complex[24];

                        for (int i = 0; i < 24; i++)
                        {
                            double f = 24.803 * Math.Pow(2, (double)i/3);
                            sampled[i] = (int)(f * 2 * BasisSpectrum.Length / samplingFrequency);
                            sel_Spectrum[i] = Spectrum[sampled[i]];
                            abs[i] = AbsorptionModels.Operations.Absorption_Coef(sel_Spectrum[i]);
                            sel_freq[i] = freq[sampled[i]];
                        }
                    }

                    public override double F(List<double> x)
                    {
                        //foreach (int i in x) if (i > 1 || i < -1) return double.PositiveInfinity;

                        //Complex[] p = new Complex[(int)Math.Floor((double)x.Count / 4)];
                        //Complex[] z = new Complex[(int)Math.Floor((double)x.Count / 4)];
                        Complex[] p = new Complex[2*pno_conj + pno_real];
                        Complex[] z = new Complex[zno_real];

                        //for (int i = 0; i < p.Length; i++) p[i] = Complex.FromPolarCoordinates(x[2*i], x[2*i + 1] * Math.PI * 2);
                        //for (int i = p.Length; i < 2*p.Length; i++) z[i - p.Length] = Complex.FromPolarCoordinates(x[2*i], x[2*i+1] * Math.PI * 2);
                        //for (int i = 0; i < p.Length; i++)
                        //{
                        //    p[i] = Complex.FromPolarCoordinates(x[2 * i], x[2 * i + 1] * Math.PI * 2);
                        //    z[i] = Complex.Conjugate(p[i]);
                        //}
                        for (int i = 0; i < pole_phase.Length; i++)
                        {
                            p[2 * i] = Complex.FromPolarCoordinates(1, pole_phase[i]);//x[i]
                            p[2 * i + 1] = Complex.Conjugate(p[2*i]);
                        }
                        for (int i = 0; i < pno_conj_phase; i++)
                        {
                            p[2 * i + pole_phase.Length] = Complex.FromPolarCoordinates(x[pole_phase.Length + 2 * i], x[pole_phase.Length + 2 * i + 1] * Math.PI * 2);
                            p[2 * i + pole_phase.Length + 1] = Complex.Conjugate(p[i]);
                        }
                        for (int i = 0; i < pno_real; i++)
                        {
                            p[i + pno_conj] = x[pno_conj + pno_conj_phase + i];
                        }

                        for (int i = 0; i < zno_real; i++)
                        {
                            z[i] = x[pno_conj + pno_conj_phase + pno_real + i];
                        }

                        //for (int i = 0; i < pole_phase.Length; i++)
                        //{
                        //    p[2 * i] = Complex.FromPolarCoordinates(x[2 * i], pole_phase[i]);
                        //    p[2 * i + 1] = Complex.Conjugate(p[i]);
                        //}
                        //for (int i = 0; i < pno_conj_phase; i++)
                        //{
                        //    p[2 * i + pole_phase.Length] = Complex.FromPolarCoordinates(x[2 * i], x[2 * i + 1] * Math.PI * 2);
                        //    p[2 * i + pole_phase.Length + 1] = Complex.Conjugate(p[i]);
                        //}
                        //for (int i = 0; i < pno_real; i++)
                        //{
                        //    p[i + 2 * pno_conj] = x[2 * pno_conj + i];
                        //}

                        //for (int i = 0; i < zno_real; i++)
                        //{
                        //    z[i] = x[2 * pno_conj + pno_real + i];
                        //}
                        
                        Complex[] a, b;
                        PZ2AB(p, z, 1, out a, out b);
                        //Complex[] spec = SpectrumFromIIR(a, b, fs, n);
                        Complex[] spec = AB_FreqResponse(new List<Complex>(b), new List<Complex>(a), sel_freq);

                        Complex[] alpha = new Complex[spec.Length];
                        double[] alpha_d = new double[spec.Length];

                        for(int i = 0; i < spec.Length; i++)
                        { 
                            alpha[i] = AbsorptionModels.Operations.Absorption_Coef(spec[i]);
                            alpha_d[i] = alpha[i].Real;
                            if (alpha[i].Real > 1 || alpha[i].Real < 0) return double.PositiveInfinity;
                        }

                        //double ans = -Math.Abs(CrossCorrelation_Coef(spec, sel_Spectrum).Real);

                        double ans = CompareFunctions(abs, alpha, sampled) - 1;
                        //if (ans > -0.9) return 1000;
                        return ans;
                    }

                    public override List<double> Gradient(List<double> x)
                    {
                        throw new NotImplementedException();
                    }

                    public override List<List<double>> Hessian(List<double> x)
                    {
                        throw new NotImplementedException();
                    }

                    public override int NumberOfVariable
                    {
                        get
                        {
                            //return 2*pno_conj + pno_real + zno_real;
                            return pno_conj_phase + pno_conj + pno_real + zno_real;
                        }
                    }
                }

                //public static void PZtoAB(Complex[] p, Complex[] z, out double[] a, out double[] b)
                //{
                //    Complex[] a1, b1;
                //    PZtoAB(p, z, out a1, out b1);
                //    a = new double[a1.Length];
                //    b = new double[b1.Length];
                //    for (int i = 0; i < a.Length; i++)
                //    {
                //        a[i] = a1[i].Magnitude;
                //        b[i] = b1[i].Magnitude;
                //    }
                //}

                //public static void ABtpPZ(out Complex[] a, out Complex[] b, Complex[] p, Complex[] z)
                //{

                //}

                public static void DW2AB(double[] d, double[] w, out double[] a, out double[] b)
                {
                    if (d.Length != w.Length) throw new Exception("Angular frequency and damping coefficients must be organized in pairs...");

                    b = new double[2] { 2 * d[0] * w[0], 0 };
                    a = new double[3] { 1, 2 * d[0] * w[0], w[0] * w[0] };

                    for (int i = 1; i < d.Length; i++)
                    {
                        double coef = 2 * d[i] * w[i];
                        double[] btemp = new double[2] { coef, 0 };
                        double[] atemp = new double[3] { 1, coef, w[0] * w[0] };

                        b = Polynomial_Add(Polynomial_Multiply(b, atemp), Polynomial_Multiply(btemp, a));
                        a = Polynomial_Multiply(a, atemp);
                    }
                }

                public static void DW2PZ(double[] d, double[] w, out Complex[] p, out double[] z, out double[] g)
                {
                    if (d.Length != w.Length) throw new Exception("Angular frequency and damping coefficients must be organized in pairs...");
                    p = new Complex[w.Length * 4];
                    z = new double[w.Length];
                    g = new double[w.Length];

                    for (int i = 0; i < d.Length; i++)
                    {
                        double _dw = -d[i] * w[i];
                        double rtwd2_1 = Math.Sqrt(_dw * _dw - w[i]);
                        p[2 * i] = _dw + rtwd2_1;
                        p[2 * i + 1] = _dw - rtwd2_1;
                        z[i] = 0;
                        g[i] = -2 * _dw;
                    }
                }

                public static void P2poly(Complex[] p, out Complex[] poly)
                {
                    poly = new Complex[p.Length + 1];

                    poly[0] = 1;

                    if (p.Length == 1) poly[1] = -p[0];
                    else
                    {
                        poly[1] = -2 * p[0] * p[1];
                        poly[2] = p[0] * p[1];
                        for (int n = 2; n < p.Length; n++)
                        {
                            for (int m = 1; m < poly.Length; m++) poly[m] += p[n] * -poly[m - 1];
                        }
                    }
                }


                public static void PZ2AB(Complex[] p, Complex[] z, double k, out Complex[] a, out Complex[] b)
                {
                    a = new Complex[p.Length + 1];
                    b = new Complex[z.Length + 1];

                    a[0] = 1;
                    b[0] = k;

                    if (p.Length == 1)
                    {
                        a[1] = -p[0];
                        b[1] = -z[0];
                    }
                    else
                    {
                        a[1] = -2 * p[0] * p[1];
                        b[1] = -2 * z[0] * z[1];
                        a[2] = p[0] * p[1];
                        b[2] = z[0] * z[1];

                        for (int n = 2; n < p.Length; n++)
                        {
                            for (int m = 1; m < a.Length; m++)
                            {
                                a[m] += p[n] * -a[m - 1];
                                b[m] += z[n] * -b[m - 1];
                            }
                        }
                    }
                }
                
                private static double[] Polynomial_Add(double[] A, double[] B)
                {
                    double[] sum = new double[Math.Max(A.Length, B.Length)];
                    int min = Math.Min(A.Length, B.Length) - 1;
                    for (int i = min; i >= 0; i--) sum[i] = A[i] + B[i];
                    return sum;
                }

                private static Complex[] Polynomial_Add(Complex[] A, Complex[] B)
                {
                    Complex[] sum = new Complex[Math.Max(A.Length, B.Length)];
                    int min = Math.Min(A.Length, B.Length)-1;
                    for (int i = min; i >= 0; i--) sum[i] = A[i] + B[i];
                    return sum;
                }

                private static double[] Polynomial_Multiply(double[] A, double[] B)
                {
                    double[] prod = new double[A.Length + B.Length - 1];
                    // Multiply two polynomials term by term
                    for (int i = 0; i < A.Length; i++)
                    {
                        for (int j = 0; j < B.Length; j++)
                            prod[i + j] += A[i] * B[j];
                    }

                    return prod;
                }

                private static Complex[] Polynomial_Multiply(Complex[] A, Complex[] B)
                {
                    Complex[] prod = new Complex[A.Length + B.Length - 1];
                    // Multiply two polynomials term by term
                    for (int i = 0; i < A.Length; i++)
                    {
                        for (int j = 0; j < B.Length; j++)
                            prod[i + j] += A[i] * B[j];
                    }

                    return prod;
                }

                //public static void OptimizeIIR(Complex[] Spectrum, int Sample_Freq, int filter_order, out double[] a, out double[] b)
                //{
                //    IIR_Spectrum_Objective obj = new IIR_Spectrum_Objective(filter_order, Spectrum, Sample_Freq);
                //    //LibOptimization.Optimization.clsOptRealGAREX SA = new LibOptimization.Optimization.clsOptRealGAREX(obj);
                //    //LibOptimization.Optimization.clsOptRealGABLX SA = new LibOptimization.Optimization.clsOptRealGABLX(obj);
                //    LibOptimization.Optimization.clsOptSimulatedAnnealing SA = new LibOptimization.Optimization.clsOptSimulatedAnnealing(obj);
                //    //LibOptimization.Optimization.clsOptNelderMead SA = new LibOptimization.Optimization.clsOptNelderMead(obj);
                //    //SA.UseEliteStrategy(10);
                //    //SA.PARAM_PopulationSize = Filter_Order * 50;
                //    SA. = 0.00001;
                //    //SA.PARAM_InitRange = 1;
                //    //SA.PARAM_ChildrenSize = 20;
                //    //SA.PARAM_MAX_ITERATION = 1000;
                //    SA.Init();
                //    SA.DoIteration();

                //    DW2AB( SA.Result.ToArray(), null, out a, out b);
                //}

                //public static void OptimizeIIR(Complex[] Spectrum, int Filter_Order, int Sample_Freq, out Complex[] a, out Complex[] b)
                //{
                //    IIR_Spectrum_Objective obj = new IIR_Spectrum_Objective(Filter_Order, Spectrum, Sample_Freq);
                //    //LibOptimization.Optimization.clsOptRealGAREX SA = new LibOptimization.Optimization.clsOptRealGAREX(obj);
                //    LibOptimization.Optimization.clsOptRealGABLX SA = new LibOptimization.Optimization.clsOptRealGABLX(obj);
                //    //SA.UseEliteStrategy(10);
                //    //SA.PARAM_PopulationSize = Filter_Order * 50;

                //    SA. = 1;
                //    SA.PARAM_ChildrenSize = 20;
                //    //SA.PARAM_MAX_ITERATION = 1000;
                //    SA.Init();
                //    SA.DoIteration();

                //    Filter_Order = obj.pzct;

                //    //Complex[] p = new Complex[(int)Math.Floor((double)SA.Result.Count / 4)];
                //    //Complex[] z = new Complex[(int)Math.Ceiling((double)SA.Result.Count / 4)];
                //    Complex[] p = new Complex[Filter_Order];
                //    Complex[] z = new Complex[Filter_Order];

                //    int pzct = Filter_Order;
                //    int pno_conj = (int)Math.Floor((double)pzct / 2);
                //    int pno_conj_phase = pno_conj - obj.pole_phase.Length;
                //    int pno_real = pzct % 2;
                //    int zno_real = pzct;

                //    for (int i = 0; i < obj.pole_phase.Length; i++)
                //    {
                //        p[2 * i] = Complex.FromPolarCoordinates(SA.Result[i], obj.pole_phase[i]);
                //        p[2 * i + 1] = Complex.Conjugate(p[2*i]);
                //    }
                //    for (int i = 0; i < pno_conj_phase; i++)
                //    {
                //        p[2 * i + obj.pole_phase.Length] = Complex.FromPolarCoordinates(SA.Result[obj.pole_phase.Length + 2 * i], SA.Result[obj.pole_phase.Length + 2 * i + 1] * Math.PI * 2);
                //        p[2 * i + obj.pole_phase.Length + 1] = Complex.Conjugate(p[i]);
                //    }
                //    for (int i = 0; i < pno_real; i++)
                //    {
                //        p[i + pno_conj] = SA.Result[pno_conj + pno_conj_phase + i];
                //    }

                //    for (int i = 0; i < zno_real; i++)
                //    {
                //        z[i] = SA.Result[pno_conj + pno_conj_phase + pno_real + i];
                //    }
                    
                //    //for (int i = 0; i < p.Length; i++) p[i] = Complex.FromPolarCoordinates(SA.Result[2 * i], SA.Result[2 * i + 1] * Math.PI * 2);
                //    //for (int i = p.Length; i < 2 * p.Length; i++) z[i-p.Length] = Complex.FromPolarCoordinates(SA.Result[2 * i], SA.Result[2 * i + 1] * Math.PI * 2);
                //    PZ2AB(p, z, 1, out a, out b);

                //    //Normalize
                //    //Complex[] spec = SpectrumFromIIR(a, b, Sample_Freq, Spectrum.Length);
                //    //double ms = 0;
                //    //foreach (Complex s in spec) ms = ((ms > s.Magnitude) ? ms : s.Magnitude);
                //    //double mb = 0;
                //    //foreach (Complex s in Spectrum) mb = ((mb > s.Magnitude) ? mb : s.Magnitude);

                //    //b[0] *= mb / ms;
                //}

                //                public static void invfreq(Complex[] Freq_Response, double[] Frequencies, int nB, int nA, out double[] b, out double[] a)
                //                {
                //                    //int Zb = 0;
                //                    int n = Math.Max(nA, nB);
                //                    int m = n + 1, mA = nA + 1, mB = nB + 1;
                //                    int nF = Frequencies.Length;
                //                    if (Frequencies.Length != Freq_Response.Length) throw new Exception("Length of Freq_Response and Frequenices must be the same");
                //                    //if nargin< 5 || isempty(W), W = ones(1, nF); endif
                //                    //if nargin< 6, iter = []; endif
                //                    //if nargin< 7  tol = []; endif
                //                    //if nargin< 8 || isempty(tr), tr = ''; endif
                //                    //if nargin< 9, plane = 'z'; endif
                //                    //if nargin< 10, varargin = {}; endif
                //                    //if iter ~=[], disp('no implementation for iter yet'),endif
                //                    //if tol ~=[], disp('no implementation for tol yet'),endif
                //                    //if (plane ~= 'z' && plane ~= 's'), disp('invfreqz: Error in plane argument'), endif

                //                    //[reg, prop] = parseparams(varargin);
                //                    //## should we normalise freqs to avoid matrices with rank deficiency ?
                //                    //bool norm = false;
                //                    //## by default, use Ordinary Least Square to solve normal equations
                //                    //string method = 'LS';

                //                    MathNet.Numerics.LinearAlgebra.Matrix<Complex> Ruu = MathNet.Numerics.LinearAlgebra.Complex.Matrix.Build.Dense(mB, mB);
                //                    MathNet.Numerics.LinearAlgebra.Matrix<Complex> Ryy = MathNet.Numerics.LinearAlgebra.Complex.Matrix.Build.Dense(nA, nA);
                //                    MathNet.Numerics.LinearAlgebra.Matrix<Complex> Ryu = MathNet.Numerics.LinearAlgebra.Complex.Matrix.Build.Dense(nA, mB);
                //                    MathNet.Numerics.LinearAlgebra.Vector<Complex> Pu = MathNet.Numerics.LinearAlgebra.Complex.Vector.Build.Dense(mB);
                //                    MathNet.Numerics.LinearAlgebra.Vector<Complex> Py = MathNet.Numerics.LinearAlgebra.Complex.Vector.Build.Dense(nA);

                //                    Complex[] s = new Complex[Freq_Response.Length];
                //                    double[] F = new double[Freq_Response.Length];

                //                    for (int i = 0; i < Freq_Response.Length; i++)
                //                    {
                //                        F[i] = (double)i * Math.PI / Freq_Response.Length;
                //                        s[i] = Complex.Exp(-F[i] * Complex.ImaginaryOne);
                //                    }

                //                    for (int k = 0; k < nF; k++)
                //                    {
                //                        Complex[] Zk = new Complex[n];
                //                        for (int i = 0; i < n; i++)
                //                        {
                //                            Zk[i] = Complex.Pow(Frequencies[k] * Complex.ImaginaryOne, i);
                //                        }
                //                        Complex aHks = Freq_Response[k] * Complex.Conjugate(Freq_Response[k]);
                //                        MathNet.Numerics.LinearAlgebra.Vector<Complex> Zkv = MathNet.Numerics.LinearAlgebra.Vector<Complex>.Build.DenseOfArray(Zk);

                //                        MathNet.Numerics.LinearAlgebra.Matrix<Complex> Rk = Zkv.ToColumnMatrix() * Zkv.ToRowMatrix();
                //                        MathNet.Numerics.LinearAlgebra.Matrix<Complex> rRk = MathNet.Numerics.LinearAlgebra.Matrix<Complex>.Build.DenseOfMatrix(Rk);
                //                        for (int x = 0; x < Rk.ColumnCount; x++) for (int y = 0; y < Rk.RowCount; y++) rRk[x, y] = rRk[x, y].Real;

                //                        Ruu = Ruu + rRk.SubMatrix(0, mB, 0, mB);
                //                        Ryy += aHks * rRk.SubMatrix(2, mA, 2, mA);
                //                        Ryu += (Freq_Response[k] * Rk.SubMatrix(2, mA, 1, mB));
                //                        for (int i = 0; i < mB; i++) Pu[i] += (Complex.Conjugate(Freq_Response[k]) * Zk[i]).Real;
                //                        for (int i = 1; i < mA; i++) Py[i] += aHks * Zk[i].Real;
                //                    }

                //                    MathNet.Numerics.LinearAlgebra.Matrix<Complex> Rr = MathNet.Numerics.LinearAlgebra.Complex.Matrix.Build.Dense(s.Length, mB+nA, 1);
                //                    MathNet.Numerics.LinearAlgebra.Vector<Complex> Zk = MathNet.Numerics.LinearAlgebra.Complex.Vector.Build.DenseOfArray(s);

                //                    for (int k = 1; k < Math.Min(nA, nB); k++)
                //                    {
                //                        Rr.SetSubMatrix(0, 1 + k, Zk.ToRowMatrix());
                //                        Rr.SetSubMatrix(0, mB + k, (-Zk.PointwiseMultiply(MathNet.Numerics.LinearAlgebra.Complex.Vector.Build.DenseOfArray(Freq_Response)).ToColumnMatrix()));
                //                        Zk = Zk.PointwiseMultiply(MathNet.Numerics.LinearAlgebra.Complex.Vector.Build.DenseOfArray(s));
                //                    }

                //                    int k;
                //                    for (k = 1 + Math.Min(nA, nB); k < Math.Max(nA, nB) - 1; k++)
                //                    {
                //                        if (k <= nB) Rr.SetSubMatrix(0, Rr.RowCount, 1 + k, 1, Zk.ToColumnMatrix());
                //                        if(k <= nA) Rr.SetSubMatrix(0, Rr.RowCount, mB + k, 1, -Zk.PointwiseMultiply(MathNet.Numerics.LinearAlgebra.Complex.Vector.Build.DenseOfArray(Freq_Response)).ToColumnMatrix());
                //                        Zk = Zk.PointwiseMultiply(MathNet.Numerics.LinearAlgebra.Complex.Vector.Build.DenseOfArray(s));
                //                    }
                //                    k++;

                //                    if(k <= nB) Rr.SetSubMatrix(0, 1 + k, Zk.ToColumnMatrix());
                //                    if(k <= nA) Rr.SetSubMatrix(0, mB + k, (-Zk.PointwiseMultiply(MathNet.Numerics.LinearAlgebra.Complex.Vector.Build.DenseOfArray(Freq_Response)).ToColumnMatrix()));
                //                    //Rr(:, mB + k) = -Zk.* H;

                ////## complex to real equation system -- this ensures real solution
                //                    Rr = Rr.SubMatrix(0, Rr.RowCount, 1, Rr.ColumnCount);

                //                    Rr = [real(Rr); imag(Rr)]; Pr = [real(H(:)); imag(H(:))];
                ////## normal equations -- keep for ref
                ////## Rn= [Ruu(1+zB:mB, 1+zB:mB), -Ryu(:, 1+zB:mB)';  -Ryu(:, 1+zB:mB), Ryy];
                ////## Pn= [Pu(1+zB:mB); -Py];

                ////## avoid scaling errors with Theta = R\P;
                ////## [Q, R] = qr([Rn Pn]); Theta = R(1:end, 1:end-1)\R(1:end, end);
                //[Q, R] = qr([Rr Pr], 0); Theta = R(1:end-1, 1:end-1)\R(1:end-1, end);
                ////## SigN = R(end, end-1);
                //                //SigN = R(end, end);

                //                B = [zeros(0, 1); Theta(1:mB)];
                //                A = [1; Theta(mB + (1:nA))];
                //            }

                public static double CompareFunctions(Complex[] f1, Complex[] f2, int[] sampled)
                {
                    if (f1.Length != f2.Length) throw new Exception("f1 and f2 must be equal in length");
                    
                    double Corr = 0;

                    //foreach (int f in sampled)
                    for(int i = 0; i < f1.Length; i++)
                    {
                        //if (f > f1.Length) continue;
                        //Corr += Math.Abs((f1[f].Magnitude - f2[f].Magnitude))/Math.Max(f1[f].Magnitude, f2[f].Magnitude);
                        Corr += Math.Pow(f1[i].Real - f2[i].Real, 2) + Math.Pow(f1[i].Imaginary - f2[i].Imaginary, 2);
                        //Corr += Math.Abs((f1[i].Real - f2[i].Real)) / Math.Max(f1[i].Real, f2[i].Real);
                        //Corr = Math.Max(Corr, Math.Abs((f1[f].Imaginary - f2[f].Imaginary)) / Math.Max(f1[f].Imaginary, f2[f].Imaginary));
                    }

                    //Corr /= sampled.Length;
                    return Corr;
                }

                public static Complex CrossCorrelation_Coef(Complex[] f1, Complex[] f2, int[] sampled)
                {
                    //if (f1.Length != f2.Length) throw new Exception("f1 and f2 must be equal in length");
                    int min = Math.Min(f1.Length, f2.Length);
                    int lost = 0;

                    Complex mf1 = 0, mf2 = 0;
                    foreach(int i in sampled)
                    {
                        if (i > min)
                        {
                            lost++;
                            continue;
                        }
                        mf1 += f1[i];
                        mf2 += f2[i];
                    }

                    mf1 /= f1.Length - lost;
                    mf2 /= f2.Length - lost;

                    Complex Corr = 0;
                    Complex sx = 0, sy = 0;
                    foreach(int f in sampled)
                    {
                        if (f > min) continue;
                        Complex x1 = f1[f] - mf1;
                        Complex x2 = f2[f] - mf2;
                        Corr += Complex.Conjugate(x1) * x2;
                        sx += f1[f];
                        sy += f2[f];
                    }

                    Corr /= Complex.Sqrt(sx * sy);
                    return Corr;
                }

                public static Complex CrossCorrelation_Coef(Complex[] f1, Complex[] f2)
                {
                    if (f1.Length != f2.Length) throw new Exception("f1 and f2 must be equal in length");
                    Complex mf1 = 0, mf2 = 0;
                    for (int i = 0; i < f1.Length; i++)
                    {
                        mf1 += f1[i];
                        mf2 += f2[i];
                    }
                    
                    mf1 /= f1.Length;
                    mf2 /= f2.Length;

                    Complex Corr = 0;
                    Complex sx = 0, sy = 0;
                    for (int f = 0; f < f1.Length; f++)
                    {
                        Complex x1 = f1[f] - mf1;
                        Complex x2 = f2[f] - mf2;
                        Corr += Complex.Conjugate(x1) * x2;
                        sx += f1[f];
                        sy += f2[f];
                    }

                    Corr /= Complex.Sqrt(sx * sy);
                    return Corr;
                }
            }
        }
    }
}