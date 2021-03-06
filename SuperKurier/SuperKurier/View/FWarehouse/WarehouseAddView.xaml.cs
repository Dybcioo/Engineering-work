﻿using DataModel;
using SuperKurier.Control;
using SuperKurier.ViewModel.FWarehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;
using SuperKurier.ViewModel;
using SuperKurier.Enums;

namespace SuperKurier.View.FWarehouse
{
    /// <summary>
    /// Logika interakcji dla klasy WarehouseAddView.xaml
    /// </summary>
    public partial class WarehouseAddView : Page
    {
        public CompanyEntities companyEntities { get; set; }
        public List<Parcel> Parcel { get; set; }
        private int reload = 0;
        private EnumTypeOfDocument documentType;

        public WarehouseAddView()
        {
            InitializeComponent();
            Parcel = new List<Parcel>();
            WarehouseGrid.DataContext = Parcel;
            companyEntities = new CompanyEntities();
        }

        private void Exit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void AddParcelToGrid_Click(object sender, RoutedEventArgs e)
        {
            var addViewModel = (WarehouseAddViewModel)DataContext;
            var parcel = addViewModel.ParcelSelected;
            if (parcel != null && parcel.id != 0)
                Parcel.Add(parcel);
            addViewModel.UpdateParcelList(Parcel, documentType);
            WarehouseGrid.DataContext = null;
            WarehouseGrid.DataContext = Parcel;
        }

        private void Btn_Buffer_Click(object sender, RoutedEventArgs e)
        {
            var doc = UpdateBuffer();
            var info = new InfoWindow();
            if(doc == null)
                info.ShowInfo($"Nie można pozostawić w buforze dokumentu bez pozycji!", $"{documentType}", "Ok");
            else
                info.ShowInfo($"Dokument {doc.code} pozostawiony w buforze!", $"{documentType}", "Ok");
            info.Close();
            Close();
        }

        private void Btn_PutOut_Click(object sender, RoutedEventArgs e)
        {
            var addViewModel = (WarehouseAddViewModel)DataContext;
            var doc = UpdateBuffer();
            var info = new InfoWindow();
            if (doc == null)
            {
                info.ShowInfo($"Nie można wystawić dokumentu bez pozycji!", $"{documentType}", "Ok");
                info.Close();
                return;
            }
            doc = companyEntities.Document.FirstOrDefault(d => d.id == doc.id);
           double summary = 0.0;
            var temp = companyEntities.ParcelMoving.Include(p => p.Parcel).Where(p => documentType == EnumTypeOfDocument.PZ ? p.idDocPZ == doc.id : p.idDocWZ == doc.id).ToList();
            if(temp.Count == 0)
            {
                info.ShowInfo($"Nie można wystawić dokumentu bez pozycji!", $"{documentType}", "Ok");
                info.Close();
                return;
            }
           foreach (var d in temp)
            {
                changeStatus(d.idParcel);
                if (documentType == EnumTypeOfDocument.PZ)
                    d.readingPZ = true;
                else if (documentType == EnumTypeOfDocument.WZ)
                    d.readingWZ = true;
                summary += (double)d.Parcel.amount;
            }
            doc.summary = (decimal)summary;
            doc.exposure = true;
            doc.idEmployee = Properties.Settings.Default.IdUser;
            companyEntities.SaveChanges();
            
            info.ShowInfo($"Dokument {doc.code} został wystawiony pomyślnie!", $"{documentType}", "Ok");
            info.Close();
            Close();
        }

        private void changeStatus(int parcelId)
        {
            var par = companyEntities.Parcel.Include(p => p.ParcelMoving).FirstOrDefault(p => p.id == parcelId);
            HistoryOfParcel history = new HistoryOfParcel()
            {
                idParcel = par.id,
                idStatus = par.idStatus,
                idWarehouse = Properties.Settings.Default.Warehouse,
                date = DateTime.Now
            };
            companyEntities.HistoryOfParcel.Add(history);
            if (par.ParcelMoving.Count == 1 && documentType == EnumTypeOfDocument.PZ)
                par.idStatus = (int)EnumParcelStatus.acceptedSender;
            else if(par.ParcelMoving.Count == 1 && documentType == EnumTypeOfDocument.WZ)
                par.idStatus = (int)EnumParcelStatus.beetwen;
            else if (par.ParcelMoving.Count == 2 && documentType == EnumTypeOfDocument.PZ)
                par.idStatus = (int)EnumParcelStatus.acceptedReciver;
            else if (par.ParcelMoving.Count == 2 && documentType == EnumTypeOfDocument.WZ)
                par.idStatus = (int)EnumParcelStatus.handed;
        }

        private void BtnTrashBin_Click(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    var temp = (Parcel)row.Item;
                    var pm = companyEntities.ParcelMoving.FirstOrDefault(pm => pm.idParcel == temp.id);
                    if (pm != null && documentType == EnumTypeOfDocument.WZ)
                        pm.readingWZ = false;
                    else if (pm != null && documentType == EnumTypeOfDocument.PZ)
                        companyEntities.ParcelMoving.Remove(pm);
                        
                    companyEntities.SaveChanges();
                    Parcel.Remove(temp);
                    break;
                }
            var addViewModel = (WarehouseAddViewModel)DataContext;
            WarehouseGrid.DataContext = null;
            WarehouseGrid.DataContext = Parcel;
            addViewModel.UpdateParcelList(Parcel, documentType);
        }

        private Document UpdateBuffer()
        {
            var addViewModel = (WarehouseAddViewModel)DataContext;
            var document = addViewModel.Document;
            var info = new InfoWindow();
            if (document == null)
            {
                if (Parcel.Count == 0)
                    return null;
                document = new Document();
                document.idTypeOfDocument = companyEntities.TypeOfDocument.FirstOrDefault(p => p.type.Equals(documentType.ToString())).id;
                document.idWarehouse = Properties.Settings.Default.Warehouse;
                document.code = $"/{DateTime.Now.Month}/{DateTime.Now.Year}";
                document.quantity = Parcel.Count;
                document.exposure = false;
                companyEntities.Document.Add(document);
                companyEntities.SaveChanges();
                document.code = document.id + document.code;
                foreach (var p in Parcel)
                {
                    ParcelMoving temp = null;
                    if (documentType == EnumTypeOfDocument.WZ)
                        temp = companyEntities.ParcelMoving.FirstOrDefault(pm => pm.idParcel == p.id && !pm.readingWZ);
                    bool isAdd = false;
                    if (temp == null)
                    {
                        isAdd = true;
                        temp = new ParcelMoving();
                    }
                    if (documentType == EnumTypeOfDocument.PZ)
                        temp.idDocPZ = document.id;
                    else
                        temp.idDocWZ = document.id;
                    temp.idParcel = p.id;
                    if(documentType == EnumTypeOfDocument.PZ)
                        temp.readingPZ = true;
                    else if(documentType == EnumTypeOfDocument.WZ)
                        temp.readingWZ = true;
                    if(isAdd)
                        companyEntities.ParcelMoving.Add(temp);
                }
                companyEntities.SaveChanges();
            }
            else
            {
                var actuallyList = addViewModel.actuallyParcelList;
                if (Parcel.Count == 0 && actuallyList.Count == 0)
                    return null;
                foreach (var rem in actuallyList)
                {
                    if (Parcel.Any(p => p.id == rem.id))
                        continue;
                    var temp = companyEntities.ParcelMoving.FirstOrDefault(p => p.idParcel == rem.id);
                    if(temp != null)
                        companyEntities.ParcelMoving.Remove(temp);
                }
                foreach (var p in Parcel)
                {
                    if (actuallyList.Any(parcel => parcel.id == p.id))
                        continue;
                    ParcelMoving temp = null;
                    if (documentType == EnumTypeOfDocument.WZ)
                        temp = companyEntities.ParcelMoving.FirstOrDefault(pm => pm.idParcel == p.id && !pm.readingWZ);
                    bool isAdd = false;
                    if (temp == null)
                    {
                        isAdd = true;
                        temp = new ParcelMoving();
                    }
                    if (documentType == EnumTypeOfDocument.PZ)
                        temp.idDocPZ = document.id;
                    else
                        temp.idDocWZ = document.id;
                    temp.idParcel = p.id;
                    if (documentType == EnumTypeOfDocument.PZ)
                        temp.readingPZ = true;
                    else if (documentType == EnumTypeOfDocument.WZ)
                        temp.readingWZ = true;
                    if(isAdd)
                        companyEntities.ParcelMoving.Add(temp);
                }
                companyEntities.SaveChanges();
                document = companyEntities.Document.FirstOrDefault(d => d.id == document.id);
                document.quantity = companyEntities.ParcelMoving.Where(p => documentType == EnumTypeOfDocument.PZ ? p.idDocPZ == document.id : p.idDocWZ == document.id).ToList().Count;
                companyEntities.SaveChanges();
            }
            return document;
        }

        private void freezePage(bool freeze)
        {
            var addViewModel = (WarehouseAddViewModel)DataContext;
            BtnAddParcel.IsEnabled = freeze;
            BtnBuffer.IsEnabled = freeze;
            BtnPutOut.IsEnabled = freeze;
            ParcelCombo.IsEnabled = freeze;
            BtnDelete.IsEnabled = freeze;
            WarehouseGrid.Columns.FirstOrDefault(c => c.Header.Equals("Opcje")).Visibility = freeze ? Visibility.Visible : Visibility.Hidden;
        }

        private void Close()
        {
            var addViewModel = (WarehouseAddViewModel)DataContext;
            Parcel = new List<Parcel>();
            WarehouseGrid.DataContext = null;
            addViewModel.VisibilityOption = Visibility.Hidden;
        }

        private void Page_LayoutUpdated(object sender, EventArgs e)
        {
            var addViewModel = (WarehouseAddViewModel)DataContext;
            if (addViewModel == null)
                return;
            if (addViewModel.VisibilityOption == Visibility.Hidden)
            {
                reload = 0;
                return;
            }
            if (reload > 0)
                return;
            if (addViewModel.actuallyParcelList != null)
            {
                Parcel = new List<Parcel>();
                foreach (var act in addViewModel.actuallyParcelList)
                    Parcel.Add(act);
            }
            WarehouseGrid.DataContext = null;
            WarehouseGrid.DataContext = Parcel;
            documentType = addViewModel.DocumentType;
            BtnPutOut.Content = $"Wystaw {documentType}";
            BtnDelete.Content = $"Usuń {documentType}";
            addViewModel.UpdateParcelList(Parcel, documentType);
            var doc = addViewModel.Document;
            if (doc != null && doc.exposure)
                freezePage(false);
            else
                freezePage(true);
            reload++;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var addViewModel = (WarehouseAddViewModel)DataContext;
            var document = addViewModel.Document;
            if (document == null)
                return;
            foreach (var parcel in companyEntities.ParcelMoving.Where(p => documentType == EnumTypeOfDocument.PZ ? p.idDocPZ == document.id : p.idDocWZ == document.id))
                companyEntities.ParcelMoving.Remove(parcel);
            companyEntities.Document.Remove(companyEntities.Document.FirstOrDefault(d => d.id == document.id));
            companyEntities.SaveChanges();
            var info = new InfoWindow();
            info.ShowInfo($"Dokument {document.code} został usunięty pomyślnie!", $"{documentType}", "Ok");
            info.Close();
            Close();
        }
    }
}
