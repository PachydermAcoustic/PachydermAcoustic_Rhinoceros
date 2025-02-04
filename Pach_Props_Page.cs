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
            
            public Pach_Props_Page()
                : base("Pachyderm Acoustic")
            {
            }

            public override bool OnActivate(bool active)
            {
                //Pach_Properties.Instance.Refresh;
                return base.OnActivate(active);
            }

            public override object PageControl
            {
                get
                {
                    return Pach_Properties_Panel.Instance;
                }
            }                   

            public override bool OnApply()
            {
                if (PachHybridControl.Instance != null) PachHybridControl.Instance.Set_Phase_Regime(Audio.Pach_SP.Filter is Audio.Pach_SP.Linear_Phase_System);
                if (Pach_Mapping_Control.Instance != null) Pach_Mapping_Control.Instance.Set_Phase_Regime(Audio.Pach_SP.Filter is Audio.Pach_SP.Linear_Phase_System);
                return base.OnApply();
            }

            public int Get_TaskPriority()
            {
                return Pach_Properties.Instance.TaskPriority();
            }

            public int Get_Geometry_Spec()
            {
                return Pach_Properties.Instance.Geometry_Spec();
            }

            public int Get_SP_Spec()
            {
                return Pach_Properties.Instance.Spatial_Optimization;
            }

            public int Get_Oct_Depth()
            {
                return Pach_Properties.Instance.Max_Polys_Per_Node;
            }

            public static string MatLibPath => Pach_Properties.Instance.Lib_Path;

            public static int VGDomain => Pach_Properties.Instance.Spatial_Depth;

            public static bool SaveResults()
            {
                return Pach_Properties.Instance.SaveResults;
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
