using Primavera.Extensibility.Integration;

namespace Cegid.CustomRibbon.UserFunctions
{
    public class CustomCode 
    {
        public ProductContext ProductContext { get; set;  }

        public void EXT_3_FNC1()
        {
            ProductContext.PSO.MensagensDialogos.MostraMensagem(StdPlatBS100.StdBSTipos.TipoMsg.PRI_SimplesOk, "Custom Function 1, called directly, executed.");
        }
        public void EXT_3_FNC2()
        {
            ProductContext.PSO.MensagensDialogos.MostraMensagem(StdPlatBS100.StdBSTipos.TipoMsg.PRI_SimplesOk, "Custom Function 2, called directly, executed.");
        }
    }
}
