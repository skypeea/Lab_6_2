using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Prism.Commands;
using RevitAPITrainingLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab_6_2
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;

        public List<FamilySymbol> Furniture { get; }
        public FamilySymbol SelectedItem { get; set; }
        public List<Level> Levels { get; }
        public Level SelectedLevel { get; set; }
        
        public DelegateCommand SaveCommand { get; }


        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            Furniture = FamilySymbolUtils.GetFurniture(commandData);
            Levels = LevelsUtils.GetLevels(commandData);
            
            SaveCommand = new DelegateCommand(OnSaveCommand);
            
           
        }

        private void OnSaveCommand()
        {
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            if (SelectedItem == null || SelectedLevel == null)
            {
                return;
            }
            RaiseHideRequest();
            FamilyInstanceUtils.CreateFamilyInstance(_commandData, SelectedItem, SelectionUtils.GetPoint(_commandData), SelectedLevel);
            RaiseCloseRequest();
        }

        public event EventHandler HideRequest;
        private void RaiseHideRequest()
        {
            HideRequest?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}
