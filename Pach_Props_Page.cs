using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        public class Pach_Props_Page : Rhino.UI.OptionsDialogPage
        {
            Pach_Properties  pprops = null;

            public Pach_Props_Page()
                : base("Pachyderm Acoustic")
            {
            }

            public override System.Windows.Forms.Control PageControl
            {
                get
                {
                    //if ((pprops == null)) pprops = new Pach_Properties();
                    return Pach_Properties.Instance;
                }
            }

            public override bool OnApply()
            {
                if (Pach_Hybrid_Control.Instance != null) Pach_Hybrid_Control.Instance.Set_Phase_Regime(Audio.Pach_SP.Filter is Audio.Pach_SP.Linear_Phase_System);
                if (Pach_Mapping_Control.Instance != null) Pach_Mapping_Control.Instance.Set_Phase_Regime(Audio.Pach_SP.Filter is Audio.Pach_SP.Linear_Phase_System);
                return base.OnApply();
            }

            public int Get_Processor_Spec()
            {
                return Pach_Properties.Instance.ProcessorCount();
            }

            public int Get_Geometry_Spec()
            {
                return Pach_Properties.Instance.Geometry_Spec();
            }

            public int Get_SP_Spec()
            {
                return Pach_Properties.Instance.SP_Spec();
            }

            public int Get_Oct_Depth()
            {
                return Pach_Properties.Instance.Oct_Depth();
            }

            public string Get_MatLib_Path()
            {
                return Pach_Properties.Instance.Lib_Path();
            }

            public int Get_VG_Domain()
            {
                return Pach_Properties.Instance.VG_Domain();
            }

            public bool Save_Results()
            {
                return Pach_Properties.Instance.SaveResults();
            }

            public override string LocalPageTitle
            {
                get
                {
                    return "Pachyderm_Acoustic";
                }
            }
        }
    }
}
