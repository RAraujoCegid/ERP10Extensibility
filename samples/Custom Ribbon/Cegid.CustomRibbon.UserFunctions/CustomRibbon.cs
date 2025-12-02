using Primavera.Extensibility.BusinessEntities.ExtensibilityService.EventArgs;
using Primavera.Extensibility.Platform.Services;
using StdPlatBS100;
using System.Drawing;


namespace Cegid.CustomRibbon.UserFunctions
{
    /*
     * Based on https://github.com/PrimaverabssDeveloper/ERP10Extensibility/tree/master/samples/Custom%20Ribbon
     * Necessary to extend plt to access services for the creation of the custom ribbon
     */
    public class CustomRibbon : Plataforma
    {
        // Instance of the Custom Code class, where functions can be directly called
        CustomCode CustomCodeObj = new CustomCode();

        #region Private Variables

        private StdBSPRibbon RibbonEvents;

        #endregion

        #region Override

        public override void DepoisDeCriarMenus(ExtensibilityEventArgs e)
        {
            base.DepoisDeCriarMenus(e);
            CustomCodeObj.ProductContext = this;

            if (RibbonEvents != null)
                RibbonEvents.Executa -= RibbonEvents_OnExecuta;

            // Register the Ribbon button.
            RibbonEvents = this.PSO.Ribbon;
            RibbonEvents.Executa += RibbonEvents_OnExecuta;

            // Can  either create the ribbon items based on user functions registered in the ERP
            CreateCustomRibbonItems_ForUserFunctions();

            // Or create ribbon items that will call functions directly from the DLLs
            CreateCustomRibbonItems_CalledDirectly();
        }

        #endregion

        #region  Private Events

        /// <summary>
        /// Event triggered when clicking on the Ribbon button.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Comando"></param>
        private void RibbonEvents_OnExecuta(string Id, string Comando)
        {
            try
            {
                switch (Id)
                {
                    // To execute functions, use PSO.FuncoesUtilizador.Executa
                    // Function must exist on the ERP and be registered with the same name
                    case RibbonConstants.cEXT1_FNC1_NAME:
                        this.PSO.FuncoesUtilizador.Executa(RibbonConstants.cEXT1_FNC1_NAME);
                        break;
                    case RibbonConstants.cEXT1_FNC2_NAME:
                        this.PSO.FuncoesUtilizador.Executa(RibbonConstants.cEXT1_FNC2_NAME);
                        break;
                    case RibbonConstants.cEXT2_FNC1_NAME:
                        this.PSO.FuncoesUtilizador.Executa(RibbonConstants.cEXT2_FNC2_NAME);
                        break;
                    case RibbonConstants.cEXT2_FNC2_NAME:
                        this.PSO.FuncoesUtilizador.Executa(RibbonConstants.cEXT2_FNC2_NAME);
                        break;

                    // You can also call directly the functions from the referenced DLL
                    // These functions do not need to be registered in the ERP
                    case RibbonConstants.cEXT3_FNC1_NAME:
                        CustomCodeObj.EXT_3_FNC1();
                        break;
                    case RibbonConstants.cEXT3_FNC2_NAME:
                        CustomCodeObj.EXT_3_FNC2();
                        break;

                }
            }
            catch (System.Exception ex)
            {
                PSO.MensagensDialogos.MostraAviso("ERROR", StdBSTipos.IconId.PRI_Informativo, ex.Message);
            }
        }

        #endregion

        #region Register the new tab inside PRIMAVERA Ribbon.

        /*
         * Creation of Ribbon items for the extensibility DLLs in a centralized way.
         * Using the user functions definitions, they must be previously registered in the ERP, with the same name
         * User can create custom tabs, groups and buttons
         */
        public void CreateCustomRibbonItems_ForUserFunctions()
        {
            // Create the default tab if it does not exist
            CriateRibbonTab(RibbonConstants.cEXT_DEFAULT_TAB_DESCRIPTION, RibbonConstants.cEXT_DEFAULT_TAB_ID, 0);

            // Creates groups for function from EXT1
            CreateGroup(RibbonConstants.cEXT1_GROUP_DESCRIPTION, RibbonConstants.cEXT_DEFAULT_TAB_ID, RibbonConstants.cEXT1_GROUP_ID);

            // Only create buttons for functions if they exist on the ERP
            if (this.PSO.FuncoesUtilizador.Existe(RibbonConstants.cEXT1_FNC1_NAME))
                CreateGroupButtonBig(RibbonConstants.cEXT_DEFAULT_TAB_ID, RibbonConstants.cEXT1_GROUP_ID, RibbonConstants.cEXT1_FNC1_NAME, RibbonConstants.cFUNC1_DESCRIPTION);

            if (this.PSO.FuncoesUtilizador.Existe(RibbonConstants.cEXT1_FNC2_NAME))
                CreateGroupButtonSmall(RibbonConstants.cEXT_DEFAULT_TAB_ID, RibbonConstants.cEXT1_GROUP_ID, RibbonConstants.cEXT1_FNC2_NAME, RibbonConstants.cFUNC2_DESCRIPTION);
            
            // Creates groups for function from EXT2
            CreateGroup(RibbonConstants.cEXT2_GROUP_DESCRIPTION, RibbonConstants.cEXT_DEFAULT_TAB_ID, RibbonConstants.cEXT2_GROUP_ID);

            if (this.PSO.FuncoesUtilizador.Existe(RibbonConstants.cEXT2_FNC1_NAME))
                CreateGroupButtonBig(RibbonConstants.cEXT_DEFAULT_TAB_ID, RibbonConstants.cEXT2_GROUP_ID, RibbonConstants.cEXT2_FNC1_NAME, RibbonConstants.cFUNC1_DESCRIPTION);

            if (this.PSO.FuncoesUtilizador.Existe(RibbonConstants.cEXT2_FNC2_NAME))
                CreateGroupButtonSmall(RibbonConstants.cEXT_DEFAULT_TAB_ID, RibbonConstants.cEXT2_GROUP_ID, RibbonConstants.cEXT2_FNC2_NAME, RibbonConstants.cFUNC2_DESCRIPTION);
        }

        /*
        * Creation of Ribbon items for referenced DLLS or Code
        */
        private void CreateCustomRibbonItems_CalledDirectly()
        {
            // Create the default tab if it does not exist
            CriateRibbonTab(RibbonConstants.cEXT_DEFAULT2_TAB_DESCRIPTION, RibbonConstants.cEXT_DEFAULT2_TAB_ID, 0);

            // No need to check if this function exists, because we will call the code directly on execution
            CreateGroup(RibbonConstants.cEXT3_GROUP_DESCRIPTION, RibbonConstants.cEXT_DEFAULT2_TAB_ID, RibbonConstants.cEXT3_GROUP_ID);
            CreateGroupButtonBig(RibbonConstants.cEXT_DEFAULT2_TAB_ID, RibbonConstants.cEXT3_GROUP_ID, RibbonConstants.cEXT3_FNC1_NAME, RibbonConstants.cFUNC1_DESCRIPTION);
            CreateGroupButtonSmall(RibbonConstants.cEXT_DEFAULT2_TAB_ID, RibbonConstants.cEXT3_GROUP_ID, RibbonConstants.cEXT3_FNC2_NAME, RibbonConstants.cFUNC2_DESCRIPTION);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Some of these properties will be override in the ribbon customizations
        /// </summary>
        /// <param name="description">Text on screen</param>
        /// <param name="tabId">Id to identify and add new items to it</param>
        /// <param name="index">Index on the ribbon</param>
        private void CriateRibbonTab(string description, string tabId, int index)
        {
            this.PSO.Ribbon.CriaRibbonTab(description, tabId, index);
        }

        /// <summary>
        /// Some of these properties will be override in the ribbon customizations
        /// </summary>
        /// <param name="description">Text on screen</param>
        /// <param name="tabId">Identifies the tab where the group will be created</param>
        /// <param name="groupId">Id to identify and add new items to it</param>
        private void CreateGroup(string description, string tabId, string groupId)
        {
            this.PSO.Ribbon.CriaRibbonGroup(tabId, description, groupId);
        }

        private void CreateGroupButtonSmall(string tabId, string groupId, string buttonId, string buttonDescription, Image buttonImage = null)
        {
            if (buttonImage == null)
                buttonImage = Properties.Resources.menus_utilizador_16;

            this.PSO.Ribbon.CriaRibbonButton(tabId, groupId, buttonId, buttonDescription, false, buttonImage);
        }

        private void CreateGroupButtonBig(string tabId, string groupId, string buttonId, string buttonDescription, Image buttonImage = null)
        {

            if (buttonImage == null)
                buttonImage = Properties.Resources.menus_utilizador_32;

            this.PSO.Ribbon.CriaRibbonButton(tabId, groupId, buttonId, buttonDescription, true, buttonImage);
        }

        #endregion
    }

    internal static class RibbonConstants
    {
        // Names of the functions, as registered in the ERP
        public const string cEXT1_FNC1_NAME = "EXT1.EXT_1_FNC1";
        public const string cEXT1_FNC2_NAME = "EXT1.EXT_1_FNC2";

        public const string cEXT2_FNC1_NAME = "EXT2.EXT_2_FNC1";
        public const string cEXT2_FNC2_NAME = "EXT2.EXT_2_FNC2";

        // Tab2
        public const string cEXT_DEFAULT_TAB_ID = "EXT_DEFAULT_TAB";
        public const string cEXT_DEFAULT_TAB_DESCRIPTION = "USER TAB 1";

        public const string cEXT_DEFAULT2_TAB_ID = "EXT_DEFAULT2_TAB";
        public const string cEXT_DEFAULT2_TAB_DESCRIPTION = "USER TAB 2";

        // Groups
        public const string cEXT1_GROUP_ID = "EXT_1_GROUP";
        public const string cEXT1_GROUP_DESCRIPTION = "EXT_1";

        public const string cEXT2_GROUP_ID = "EXT_2_GROUP";
        public const string cEXT2_GROUP_DESCRIPTION = "EXT_2";

        public const string cEXT3_GROUP_ID = "EXT_3_GROUP";
        public const string cEXT3_GROUP_DESCRIPTION = "EXT_3";

        // Buttons
        public const string cEXT3_FNC1_NAME = "EXT_3_FNC1";
        public const string cEXT3_FNC2_NAME = "EXT_3_FNC2";

        public const string cFUNC1_DESCRIPTION = "FUNC1";
        public const string cFUNC2_DESCRIPTION = "FUNC2";
    }
}
