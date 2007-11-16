// SelectTablesIce.cs - class for ice communication
//
// Author: Daniel KUpka <kupkd9am@post.cz>
//
// Copyright (c) 2007 Daniel Kupka
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules;
using System.Text.RegularExpressions;
using System.Collections;
using Ferda.Modules.Boxes;
using Ferda.ModulesManager;
using Ferda.FrontEnd.AddIns;
using System.Resources;
using System.Reflection;
using PropertyInfo=Ferda.Modules.PropertyInfo;
using Ferda.FrontEnd.AddIns.Common.MyIce;
using Ferda.FrontEnd.AddIns.FormGenerator;
using System.Windows.Forms;
using Interpreter;
using Ice;

namespace Ferda.FrontEnd.AddIns.FormGenerator.MyIce
{
  //protected BoxModuleI _boxModule;
    /// <summary>
    /// Class for ice communication
    /// </summary>
    class FormGeneratorIce : ModuleForInteractionIce
    {
        #region Private variables

        private VariableServices services;

        /// <summary>
        /// IBoxModule of Wizard box, from which start the scenario
        /// </summary>
        private IBoxModule Wizard_box;

        /// <summary>
        /// If it is the first action Run 
        /// </summary>
        private bool firstRun;

        /// <summary>
        /// Bax which was controlled
        /// </summary>
        private IBoxModule Controlled_box;

        /// <summary>
        /// Language code
        /// </summary>
        private string language;

        /// <summary>
        /// Non-modal dialog
        /// </summary>
        UserControl control;

        /// <summary>
        /// Tendention of relation between advice
        /// and user action
        /// </summary>
        private bool tendency;

        /// <summary>
        /// Actually active box of scenario
        /// </summary>
        private IBoxModule Active_box;

        /// <summary>
        /// Last value of controlled property
        /// </summary>
        private string oldPropertyValue;

        /// <summary>
        /// Last effered value of property
        /// </summary>
        private string oldOfferedValue;

        /// <summary>
        /// Last controlled property name
        /// </summary>
        private string oldPropertyName;

        /// <summary>
        /// Name of last active form
        /// </summary>
        private string oldFormName;

        /// <summary>
        /// Variant of last active form
        /// </summary>
        private int oldFormVariant;

        private int form_index;

        /// <summary>
        /// L10n resource manager
        /// </summary>
      //  private ResourceManager resManager;

        /// <summary>
        /// L10n string, for now en-US or cs-CZ
        /// </summary>
       // private string localizationString;

        /// <summary>
        /// Resulting string with XML content
        /// </summary>
        //string  returnString;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="ownerOfAddIn">Owner of addin</param>
        public FormGeneratorIce(Ferda.FrontEnd.AddIns.IOwnerOfAddIn ownerOfAddIn):
            base(ownerOfAddIn, null)
        {
            this.ownerOfAddIn = ownerOfAddIn;
            this.oldFormName = "";
            this.oldPropertyName = "";
            this.oldOfferedValue = "";
            this.oldPropertyValue = "";
            this.oldFormVariant = 0;
            this.tendency = false;
            this.Controlled_box = null;
            this.control = null;
            this.firstRun = true;
            this.services = new VariableServices();

            //setting the ResManager resource manager and localization string
            // resManager = new ResourceManager("Ferda.FrontEnd.AddIns.MultiSelectStrings.Localization_en-US",
            //Assembly.GetExecutingAssembly());
            //localizationString = "en-US";
        }

        #endregion


        /// <summary>
        /// Gets a list of sockets needed to be connected in order for the module to
        /// work
        /// </summary>
        /// <param name="__current">ICE stuff</param>
        /// <returns>List of socket names for module to work propertly</returns>
        public override string[] getNeededConnectedSockets(Ice.Current __current)
        {
            return new string[] {  };
        }

        /// <summary>
        /// Gets accepted box types
        /// </summary>
        /// <param name="__current">Ice context</param>
        /// <returns>Array of boxtypes</returns>
        public override Ferda.Modules.BoxType[] getAcceptedBoxTypes(Ice.Current __current)
        {
            Modules.BoxType boxType = new Modules.BoxType();
            boxType.neededSockets = new Modules.NeededSocket[0];
            //boxType.functionIceId = "::Ferda::Modules::Boxes::DataPreparation::DatabaseFunctions";
   boxType.functionIceId = "::Ferda::Modules::Boxes::Wizards::Wizard::WizardFunctions";
            return new Modules.BoxType[] { boxType };
        }

        /// <summary>
        /// Gets hint to the module according for a specified localization
        /// </summary>
        /// <param name="localePrefs">Localization preferences</param>
        /// <param name="__current">Some ICE stuff</param>
        /// <returns>Localized hint</returns>
        public override string getHint(string[] localePrefs, Ice.Current __current)
        {
            //Localize(localePrefs);
            //return resManager.GetString("DataBaseInfoModule");
            if (this.language == "en-US")
                            return "Run scenario";
            else return "Spusù scÈn·¯";
        }

        /// <summary>
        /// Gets label of the module according to the localization
        /// </summary>
        /// <param name="localePrefs">Localization specification</param>
        /// <param name="__current">Some ICE stuff</param>
        /// <returns>Localized label</returns>
        public override string getLabel(string[] localePrefs, Ice.Current __current)
        {
           // Localize(localePrefs);
           // return resManager.GetString("DataBaseInfoModule");
            if (this.language == "en-US")
                              return "Run scenario";
            else return "Spusù scÈn·¯";

        }

        /// <summary>
        /// The method find box (IBoxModule) with specific user label or box type
        /// between Start_box ancestors.
        /// </summary>
        /// <param name="Start_box">The box from which starts the finding</param>
        /// <param name="name">Box name or box type</param>
        /// <param name="is_label">If name is box label or box type</param>
        /// <returns>IBoxModule of founded box or null</returns>
        private IBoxModule FindBox(IBoxModule Start_box, string name, bool is_label)
        {
          Queue box_queue = new Queue();
          int project_identifier;
          IBoxModule box = null;
          IBoxModule box_module = null;
            
            box_queue.Enqueue(Start_box.ProjectIdentifier);

           do
           {
               project_identifier = (int)box_queue.Dequeue();
               box_module = ownerOfAddIn.ProjectManager.Archive.GetBoxByProjectIdentifier(project_identifier);

               if ( ((is_label) && (box_module.UserName == name)) ||
                   ((!is_label) && (box_module.MadeInCreator.Identifier == name)) )
               {
                   box = box_module;
                   break;
               }

               for (int i = 0; i < box_module.ConnectionsFrom().Count; i++)
               {
                   project_identifier = box_module.ConnectionsFrom()[i].ProjectIdentifier;

                   if (!box_queue.Contains(project_identifier))
                       box_queue.Enqueue(project_identifier);
               }
 
           }
           while (box_queue.Count > 0);

            return box;
        }

        /// <summary>
        /// Method react on property changes made by user.
        /// </summary>
        /// <param name="box_name">User name of Ferdas box, where the property value is changed</param>
        /// <returns>true if the control passed good</returns>
        private bool CheckFor_changes(string box_name)
        {
            if (this.Controlled_box != null)
            {
                string newValue = "";
//                string newValue = this.Controlled_box.GetPropertyString(this.oldPropertyName).ToString();
                // TODO get this code to separated method
                try
                {
                    PropertyValue PropertyValue;
                    string PropertyType = "";
                    float numberValue = 0;

                    PropertyValue = Controlled_box.GetPropertyOther(this.oldPropertyName);
                    PropertyType = PropertyValue.ToString();

                    switch (PropertyType)
                    {
                        case "Ferda.Modules.DoubleTI": numberValue = (float)Controlled_box.GetPropertyDouble(this.oldPropertyName);
                            newValue = numberValue.ToString();
                            break;

                        case "Ferda.Modules.LongTI": numberValue = (float)Controlled_box.GetPropertyLong(this.oldPropertyName);
                            newValue = numberValue.ToString();
                            break;

                        case "Ferda.Modules.IntTI": numberValue = (float)Controlled_box.GetPropertyInt(this.oldPropertyName);
                            newValue = numberValue.ToString();
                            break;

                        case "Ferda.Modules.FloatTI": numberValue = (float)Controlled_box.GetPropertyFloat(this.oldPropertyName);
                            newValue = numberValue.ToString();
                            break;

                        case "Ferda.Modules.ShortTI": numberValue = (float)Controlled_box.GetPropertyShort(this.oldPropertyName);
                            newValue = numberValue.ToString();
                            break;

                        case "Ferda.Modules.StringTI": newValue = Controlled_box.GetPropertyString(this.oldPropertyName);
                            break;
                    }

                }
                catch (Ice.UserException)
                {
                 /*   if (this.language == "en-US")
                        MessageBox.Show(form.UserName + ": Bad property name  '" + path_parts[1] + "'  ", "Error",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        MessageBox.Show(form.UserName + ": äpatnÈ pojmenov·nÌ vlastnosti  '" + path_parts[1] + "'  ", "Chyba",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);

                    break;*/
                }

                try
                {
                    object variableValue = services.getValue(this.oldOfferedValue);
                    this.oldOfferedValue = variableValue.ToString();
                }
                catch (NullReferenceException)
                { }

                if ((!this.tendency) && (newValue != this.oldOfferedValue))
                {
                  if (this.language == "en-US")
                    MessageBox.Show(box_name + " : You don't respect offered property value or tendency \n Scenario must be stopped", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                  else
                    MessageBox.Show(box_name + " : Nerespektujete hodnotu vlastnosti nebo tendenci \n Vykon·v·nÌ scÈn·¯e musÌ b˝t p¯eruöeno", "Chyba",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return false;
                }
                else if (this.tendency)
                {
                  float _offered;
                  float _old;
                  float _new;

                    try
                    {
                        _offered = float.Parse(this.oldOfferedValue);
                        _old = float.Parse(this.oldPropertyValue);
                        _new = float.Parse(newValue);
                    }

                    catch (FormatException)
                    {
                        if (this.language == "en-US")
                         MessageBox.Show(box_name + ": Comparing string values", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else
                         MessageBox.Show(box_name + ": Porovn·v·nÌ ¯etÏzcov˝ch hodnot", "Chyba",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return false;
                    }

                    if (((_offered > _old) && (_new < _old)) ||
                         ((_offered < _old) && (_new > _old)))
                    {
                        if (this.language == "en-US")
                          MessageBox.Show(box_name + ": You don't respect tendency \n Scenario must be stopped", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else
                          MessageBox.Show(box_name + ": Nerespektujete tendenci \n Vykon·v·nÌ scÈn·¯e musÌ b˝t p¯eruöeno", "Chyba",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return false;
                    }

                }
            }
            return true;
        }

        /// <summary>
        /// Method run scenario. Method include cycle, that can be leaved
        /// when some box has not successor or unmodal dialog is diplayed.
        /// after user action control have again this method.
        /// </summary>
        private void run_scenario()
        {
          FormGenerator.WizardFormGenerator gen = null;

            while (true)
            {
                IBoxModule form = null;
                int length = (Active_box.ConnectedTo()).Length;

                if (length == 0) break;

                if (this.oldFormName != "")
                {
                    int i;

                    for (i = 0; i < length; i++)
                    {
                        form = (Active_box.ConnectedTo())[i];
                        if (form.UserName == this.oldFormName) break;
                    }

                    if (i == length)
                    {
                        if (this.language == "en-US")
                          MessageBox.Show(Active_box.UserName + ": Bad successor definition  '" + this.oldFormName + "'  ", "Error",
                                         MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else
                          MessageBox.Show(Active_box.UserName + ": äpatn· definice n·slednÌka  '" + this.oldFormName + "'  ", "Chyba",
                                         MessageBoxButtons.OK, MessageBoxIcon.Error);

                        break;
                    }

                }
                else form = (Active_box.ConnectedTo())[0];

                string box_type = form.MadeInCreator.Identifier;

                if (box_type == "Wizards.WizardForm")
                {
                    if (!CheckFor_changes(form.UserName)) break;

                    string editor = form.GetPropertyString("FormEditor");

                    length = form.ConnectedTo().Length;

                    services.setValueP("$FORM_INDEX", this.form_index);
                    services.setValueP("$HISTORY[" + form_index.ToString() + "]",
                                        form.UserName + "_" + this.oldFormVariant);          
                    this.form_index++;

                    if (editor == "") editor = "<form ID=\"1\"><mainarea> </mainarea></form>";

                    System.Windows.Forms.DialogResult result;
                    if (length == 0)
                    {
                        gen = new WizardFormGenerator(editor, 200, "Back_OK", this.oldFormVariant, this.services);
                        result = gen.GenerateForm();
                        if (result == System.Windows.Forms.DialogResult.Cancel) break;
                    }
                    else
                    {
                        string parentbox_type = Active_box.MadeInCreator.Identifier;

                        if (parentbox_type == "Wizards.Wizard")
                            gen = new WizardFormGenerator(editor, 200, "Next_STOP", this.oldFormVariant, this.services);
                        else 
                            gen = new WizardFormGenerator(editor, 200, "Back_Next_STOP", this.oldFormVariant, this.services);

                         result = gen.GenerateForm();
                    }

                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        Active_box = form;
                        this.oldFormName = gen.ReturnFormName;
                        this.oldFormVariant = gen.ReturnFormVariant;
                        Controlled_box = null;
                        continue;
                    }
                    else if (result == System.Windows.Forms.DialogResult.Abort)
                    {
                      IBoxModule parent = form;

                        while (result == System.Windows.Forms.DialogResult.Abort)
                        {
                            if (parent.ConnectionsFrom().Count == 0)
                            {
                                result = System.Windows.Forms.DialogResult.Ignore;
                                break;
                            }

                            parent = parent.ConnectionsFrom()[0];
                            parent = this.FindBox(parent, "Wizards.WizardForm", false);

                            if (parent != null)
                            {
                                string content = parent.GetPropertyString("FormEditor");

                                if (parent.ConnectionsFrom()[0].MadeInCreator.Identifier == "Wizards.Wizard")
                                    gen = new WizardFormGenerator(content, 200, "Return_STOP", 0, this.services);
                                else
                                    gen = new WizardFormGenerator(content, 200, "Back_Return_STOP", 0, this.services);

                                result = gen.GenerateForm();

                            }
                            else
                            {
                                result = System.Windows.Forms.DialogResult.Ignore;
                                break;
                            }
                        }
                        if (result == System.Windows.Forms.DialogResult.Cancel) break;

                        else //return to actual state 
                        {

                            continue;    
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else if (box_type == "Wizards.WizardAction")
                {
                    string path = gen.ReturnPath;

                    if ((path == null) || (path == ""))
                    {
                        Active_box = form;
                        this.oldFormName = "";
                        this.oldPropertyName = "";
                        //this.oldFormVariant = 0;
                        continue;
                    }

                    PathInterpreter divide_path = new PathInterpreter(path);
                    string[] path_parts = divide_path.splitPath();

                    IBoxModule edited_box = FindBox(Wizard_box, path_parts[0], true);
                    this.Controlled_box = edited_box;
                    this.oldPropertyName = path_parts[1];

                    if (edited_box == null)
                    {
                        if (this.language == "en-US")
                        MessageBox.Show(form.UserName + ": Bad name of controlled box  '" + path_parts[0]+"' ", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else
                        MessageBox.Show(form.UserName + ": äpatnÈ jmÈno kontrolovanÈ krabiËky  '" + path_parts[0] + "' ", "Chyba",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                        break;
                    }
                    string Value = "";

                    try
                    {
                        PropertyValue PropertyValue;
                        string PropertyType = "";
                        float numberValue = 0;
    
                        PropertyValue = edited_box.GetPropertyOther(path_parts[1]);
                            PropertyType = PropertyValue.ToString();

                        switch (PropertyType)
                        {
                            case "Ferda.Modules.DoubleTI" : numberValue = (float) edited_box.GetPropertyDouble(path_parts[1]);
                                                            Value = numberValue.ToString();
                                                            break;

                            case "Ferda.Modules.LongTI": numberValue = (float)edited_box.GetPropertyLong(path_parts[1]);
                                                            Value = numberValue.ToString();
                                                            break;

                           case "Ferda.Modules.IntTI": numberValue = (float)edited_box.GetPropertyInt(path_parts[1]);
                                                            Value = numberValue.ToString();
                                                            break;

                           case "Ferda.Modules.FloatTI": numberValue = (float)edited_box.GetPropertyFloat(path_parts[1]);
                                                           Value = numberValue.ToString();
                                                            break;

                           case "Ferda.Modules.ShortTI": numberValue = (float)edited_box.GetPropertyShort(path_parts[1]);
                                                            Value = numberValue.ToString();
                                                            break;

                           case "Ferda.Modules.StringTI": Value = edited_box.GetPropertyString(path_parts[1]);
                                                            break;
                        }                      

                    }
                    catch (Ice.UserException)
                    {
                       if (this.language == "en-US")
                        MessageBox.Show(form.UserName + ": Bad property name  '" + path_parts[1] + "'  ", "Error",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                       else
                        MessageBox.Show(form.UserName + ": äpatnÈ pojmenov·nÌ vlastnosti  '" + path_parts[1] + "'  ", "Chyba",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);

                        break;
                    }

                    this.oldOfferedValue = path_parts[2];
                    this.oldPropertyValue = Value;  

                    if (path_parts[3] == "Y") this.tendency = true;
                    else this.tendency = false;

                    length = form.ConnectedTo().Length;

                    try
                    {
                        object variableValue = services.getValue(this.oldOfferedValue);
                        path_parts[2] = variableValue.ToString();
                    }
                    catch (NullReferenceException)
                    { }


                    string NP = path_parts[0] + "->" + path_parts[1] + "->" +
                                Value + "->" + path_parts[2]; 

                    if (length == 0)
                    {
                        control = new UserControl(NP, "OK");
                    }
                    else
                    {
                        control = new UserControl(NP, "Next");
                        control.Next += new EventHandler(Next_Accept);
                        control.Stop += new EventHandler(Stop_Accept);
                        Active_box = form;//Active_box.ConnectedTo()[0];
                        this.oldFormName = "";
                    }

                    control.Show();
                    break;
                }
                else if (box_type == "Wizards.WizardGenerated")
                {
                    if (!CheckFor_changes(form.UserName)) break;

                    length = form.ConnectedTo().Length;

                    if (length == 0)
                    {
                        break;
                    }

                    float NumberOfHypotheses = 0;
                   IBoxModule fourfold_box = FindBox(Wizard_box, "GuhaMining.Tasks.FourFold", false);

                   if (fourfold_box != null)
                   {
                       int IntNumberRuns = (int)fourfold_box.GetPropertyInt("NumberRuns");
                       int minusOne = IntNumberRuns - 1;

                       this.services.setValueP("$RUN", (float)IntNumberRuns);

                       string NHIdentifier = "$NUM_HYPOTHESES[" + IntNumberRuns.ToString() + "]";

                       NumberOfHypotheses = (float) fourfold_box.GetPropertyLong("NumberOfHypotheses");
                       this.services.setValueP(NHIdentifier, NumberOfHypotheses);

                       if (this.oldPropertyName != "")
                       {
                           string pathIdentifier = this.oldPropertyName + "[" + IntNumberRuns.ToString() + "]";

                           try
                           {
                               float value = (float)this.services.getValue("$" + this.oldPropertyName + "[" + minusOne.ToString() + "]");
                               this.services.setValueP("$"  + this.oldPropertyName + "[" + IntNumberRuns.ToString() + "]", float.Parse(this.oldPropertyValue));
                           }
                           catch (NullReferenceException)
                           {
                               this.services.setValueP("$" + this.oldPropertyName + "[" + minusOne.ToString() + "]", 0);
                           }
                       }

                   }

                    string right_successor;

                    if (NumberOfHypotheses > (float) this.services.getValue("$TOP_LIMIT"))
                       right_successor = form.GetPropertyString("ManyHypotheses");
                   else if (NumberOfHypotheses < (float) this.services.getValue("$BOTTOM_LIMIT"))
                       right_successor = form.GetPropertyString("FewHypotheses");
                    else
                       right_successor = form.GetPropertyString("AcceptablyHypotheses");

                   string[] succ_parts = right_successor.Split('_');
                    

                    if (succ_parts.Length != 2)
                    {
                        if (this.language == "en-US")
                        MessageBox.Show(form.UserName + ": Successor box name or variant in  '" + right_successor + "'  has bad format ", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else
                        MessageBox.Show(form.UserName + ": JmÈno n·slednÌka nebo varianty v  '" + right_successor + "'  m· öpatn· form·t ", "Chyba",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                        break;
                    }

                    int variant;

                    try
                    {
                        variant = int.Parse(succ_parts[1]);
                    }
                    catch (FormatException)
                    {
                        if (this.language == "en-US")
                        MessageBox.Show(form.UserName + ": Form variant in  '" + right_successor + "'  has bad format", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else
                        MessageBox.Show(form.UserName + ": Varianta formul·¯e v  '" + right_successor + "'  m· öpatn˝ form·t", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                        break;
                    }

                    Active_box = form;
                    this.oldFormName = succ_parts[0];
                    this.oldFormVariant = variant;
                    this.Controlled_box = null;
                }

            }

        }


        #region Icerun

        /// <summary>
        /// The method is called by te Ice framework when a module for interaction
        /// is to be displayed. More here are set some WizardLanguage non-editable
        /// variables.
        /// </summary>
        /// <param name="boxModuleParam">Box that is executing this module for interaction</param>
        /// <param name="localePrefs">Localization preferences</param>
        /// <param name="manager">Proxy address of the modules manager</param>
        /// <param name="__current">Ice context</param>
        /// 
        public override void run(Ferda.Modules.BoxModulePrx boxModuleParam, string[] localePrefs, ManagersEnginePrx manager, Ice.Current __current)
        {
            this.oldFormName = "";
            this.oldPropertyName = "";
            this.oldFormVariant = 0;
            this.oldOfferedValue = "";
            this.oldPropertyValue = "";
            this.tendency = false;
            this.Controlled_box = null;
            string id = Ice.Util.identityToString(boxModuleParam.ice_getIdentity());

            Wizard_box = 
                ownerOfAddIn.ProjectManager.ModulesManager.GetIBoxModuleByIdentity(id);

              Active_box = Wizard_box;

              IBoxModule table_box = FindBox(Wizard_box, "DataPreparation.DataSource.DataTable", false);
              if (table_box != null)
              {
                  float number_rows = (float) table_box.GetPropertyLong("RecordsCount");
                     
                 this.services.setValueP("$NUMBER_ROWS", number_rows);
              }

              this.language = localePrefs[0];
              this.form_index = 0;

              IBoxModule fourfold_box = FindBox(Wizard_box, "GuhaMining.Tasks.FourFold", false);

              if (fourfold_box != null)
              {
                  int number_runs = (int)fourfold_box.GetPropertyInt("NumberRuns");

                  if (firstRun)
                  {
                   string NHIdentifier = "$NUM_HYPOTHESES[";
                   for (int i = 0; i <= number_runs; i++)
                       this.services.setValueP(NHIdentifier + i.ToString() + "]", 0);

                   firstRun = false;
                  }
              }


              run_scenario();
            
            }
    
        #endregion

        /// <summary>
        /// Standard button_click method.
        /// Closes non-modal dialog and 
        /// returns to cycle in run_scenario().
        /// </summary>
        public void Next_Accept(object sender, EventArgs e)
        {
            control.Dispose();

            this.run_scenario();
        }

        /// <summary>
        /// Standard button_click method.
        /// Closes non-modal dialog. 
        /// </summary>
        public void Stop_Accept(object sender, EventArgs e)
        {
            control.Dispose();
        }

        #region Other private methods

        /// <summary>
        /// Handler of disposing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void control_Disposed(object sender, EventArgs e)
        {
            /*Ferda.FrontEnd.AddIns.FormEditor.WizardFormEditor control =
                (Ferda.FrontEnd.AddIns.FormEditor.WizardFormEditor)sender;

            this.returnString = control.ReturnString;*/
        }

        #endregion
    }
}
