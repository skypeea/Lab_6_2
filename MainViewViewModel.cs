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

namespace Lab_6_3
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;

        public List<FamilySymbol> Furniture { get; }
        public FamilySymbol SelectedItem { get; set; }
        public List<Level> Levels { get; }
        public Level SelectedLevel { get; set; }
        public int Quantity { get; set; }
        public XYZ Point1 { get; set; }
        public XYZ Point2 { get; set; }


        public DelegateCommand SaveCommand { get; }


        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            Furniture = FamilySymbolUtils.GetFamilySymbols(commandData);
            Levels = LevelsUtils.GetLevels(commandData);
            Point1 = SelectionUtils.GetPoint(commandData);
            Point2 = SelectionUtils.GetPoint(commandData);
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
            XYZ direction = Point2 - Point1;
            double step = direction.GetLength()/Quantity;
            direction = direction.Normalize();
            for (int i =0; i<Quantity; i++)
            {
                FamilyInstanceUtils.CreateFamilyInstance(_commandData, SelectedItem,(Point1+direction*i*step+0.5*direction*step), SelectedLevel);
            }
            
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
