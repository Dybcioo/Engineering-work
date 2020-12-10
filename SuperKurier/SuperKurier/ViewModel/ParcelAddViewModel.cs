using Caliburn.Micro;
using DataModel;
using Microsoft.Maps.MapControl.WPF;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using SuperKurier.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZXing;

namespace SuperKurier.ViewModel
{
    public class ParcelAddViewModel : ParcelViewModel, IDataErrorInfo
    {
        private CompanyEntities CompanyEntities = new CompanyEntities();
        private string _senderFirstName;
        public string SenderFirstName
        {
            get { return _senderFirstName; }
            set
            {
                if (value != _senderFirstName)
                {
                    _senderFirstName = value;
                    OnPropertChanged("SenderFirstName");
                    OnPropertChanged("SenderLastName");
                    OnPropertChanged("SenderCompanyNIP");
                    OnPropertChanged("SenderCompanyName");
                }
            }
        }
        private string _senderLastName;
        public string SenderLastName
        {
            get { return _senderLastName; }
            set
            {
                if (value != _senderLastName)
                {
                    _senderLastName = value;
                    OnPropertChanged("SenderFirstName");
                    OnPropertChanged("SenderLastName");
                    OnPropertChanged("SenderCompanyNIP");
                    OnPropertChanged("SenderCompanyName");
                }
            }
        }
        private string _senderCompanyName;
        public string SenderCompanyName
        {
            get { return _senderCompanyName; }
            set
            {
                if (value != _senderCompanyName)
                {
                    _senderCompanyName = value;
                    OnPropertChanged("SenderFirstName");
                    OnPropertChanged("SenderLastName");
                    OnPropertChanged("SenderCompanyNIP");
                    OnPropertChanged("SenderCompanyName");
                }
            }
        }
        private string _senderCompanyNIP;
        public string SenderCompanyNIP
        {
            get { return _senderCompanyNIP; }
            set
            {
                if (value != _senderCompanyNIP)
                {
                    _senderCompanyNIP = value;
                    OnPropertChanged("SenderFirstName");
                    OnPropertChanged("SenderLastName");
                    OnPropertChanged("SenderCompanyNIP");
                    OnPropertChanged("SenderCompanyName");
                }
            }
        }
        private string _senderTel;
        public string SenderTel
        {
            get { return _senderTel; }
            set
            {
                if (value != _senderTel)
                {
                    _senderTel = value;
                    OnPropertChanged("SenderTel");
                }
            }
        }
        private string _senderCountry;
        public string SenderCountry
        {
            get { return _senderCountry; }
            set
            {
                if (value != _senderCountry)
                {
                    _senderCountry = value;
                    OnPropertChanged("SenderCountry");
                }
            }
        }
        private string _senderCity;
        public string SenderCity
        {
            get { return _senderCity; }
            set
            {
                if (value != _senderCity)
                {
                    _senderCity = value;
                    OnPropertChanged("SenderCity");
                }
            }
        }
        private string _senderPostalCode;
        public string SenderPostalCode
        {
            get { return _senderPostalCode; }
            set
            {
                if (value != _senderPostalCode)
                {
                    _senderPostalCode = value;
                    OnPropertChanged("SenderPostalCode");
                }
            }
        }
        private string _senderStreet;
        public string SenderStreet
        {
            get { return _senderStreet; }
            set
            {
                if (value != _senderStreet)
                {
                    _senderStreet = value;
                    OnPropertChanged("SenderStreet");
                }
            }
        }
        private string _senderNumberOfHouse;
        public string SenderNumberOfHouse
        {
            get { return _senderNumberOfHouse; }
            set
            {
                if (value != _senderNumberOfHouse)
                {
                    _senderNumberOfHouse = value;
                    OnPropertChanged("SenderNumberOfHouse");
                }
            }
        }
        private string _receiverFirstName;
        public string ReceiverFirstName
        {
            get { return _receiverFirstName; }
            set
            {
                if (value != _receiverFirstName)
                {
                    _receiverFirstName = value;
                    OnPropertChanged("ReceiverFirstName");
                    OnPropertChanged("ReceiverLastName");
                    OnPropertChanged("ReceiverCompanyNIP");
                    OnPropertChanged("ReceiverCompanyName");
                }
            }
        }
        private string _receiverLastName;
        public string ReceiverLastName
        {
            get { return _receiverLastName; }
            set
            {
                if (value != _receiverLastName)
                {
                    _receiverLastName = value;
                    OnPropertChanged("ReceiverFirstName");
                    OnPropertChanged("ReceiverLastName");
                    OnPropertChanged("ReceiverCompanyNIP");
                    OnPropertChanged("ReceiverCompanyName");
                }
            }
        }
        private string _receiverCompanyName;
        public string ReceiverCompanyName
        {
            get { return _receiverCompanyName; }
            set
            {
                if (value != _receiverCompanyName)
                {
                    _receiverCompanyName = value;
                    OnPropertChanged("ReceiverFirstName");
                    OnPropertChanged("ReceiverLastName");
                    OnPropertChanged("ReceiverCompanyNIP");
                    OnPropertChanged("ReceiverCompanyName");
                }
            }
        }
        private string _receiverCompanyNIP;
        public string ReceiverCompanyNIP
        {
            get { return _receiverCompanyNIP; }
            set
            {
                if (value != _receiverCompanyNIP)
                {
                    _receiverCompanyNIP = value;
                    OnPropertChanged("ReceiverFirstName");
                    OnPropertChanged("ReceiverLastName");
                    OnPropertChanged("ReceiverCompanyNIP");
                    OnPropertChanged("ReceiverCompanyName");
                }
            }
        }
        private string _receiverTel;
        public string ReceiverTel
        {
            get { return _receiverTel; }
            set
            {
                if (value != _receiverTel)
                {
                    _receiverTel = value;
                    OnPropertChanged("ReceiverTel");
                }
            }
        }
        private string _receiverCountry;
        public string ReceiverCountry
        {
            get { return _receiverCountry; }
            set
            {
                if (value != _receiverCountry)
                {
                    _receiverCountry = value;
                    OnPropertChanged("ReceiverCountry");
                }
            }
        }
        private string _receiverCity;
        public string ReceiverCity
        {
            get { return _receiverCity; }
            set
            {
                if (value != _receiverCity)
                {
                    _receiverCity = value;
                    OnPropertChanged("ReceiverCity");
                }
            }
        }
        private string _receiverPostalCode;
        public string ReceiverPostalCode
        {
            get { return _receiverPostalCode; }
            set
            {
                if (value != _receiverPostalCode)
                {
                    _receiverPostalCode = value;
                    OnPropertChanged("ReceiverPostalCode");
                }
            }
        }
        private string _receiverStreet;
        public string ReceiverStreet
        {
            get { return _receiverStreet; }
            set
            {
                if (value != _receiverStreet)
                {
                    _receiverStreet = value;
                    OnPropertChanged("ReceiverStreet");
                }
            }
        }
        private string _receiverNumberOfHouse;
        public string ReceiverNumberOfHouse
        {
            get { return _receiverNumberOfHouse; }
            set
            {
                if (value != _receiverNumberOfHouse)
                {
                    _receiverNumberOfHouse = value;
                    OnPropertChanged("ReceiverNumberOfHouse");
                }
            }
        }
        private string _parcelWeight;
        public string ParcelWeight
        {
            get { return _parcelWeight ?? "1"; }
            set
            {
                if (value != _parcelWeight)
                {
                    _parcelWeight = value;
                    OnPropertChanged("ParcelWeight");
                }
            }
        }
        private string _parcelHeight;
        public string ParcelHeight
        {
            get { return _parcelHeight ?? "1"; }
            set
            {
                if (value != _parcelHeight)
                {
                    _parcelHeight = value;
                    OnPropertChanged("ParcelHeight");
                }
            }
        }
        private string _parcelWidth;
        public string ParcelWidth
        {
            get { return _parcelWidth ?? "1"; }
            set
            {
                if (value != _parcelWidth)
                {
                    _parcelWidth = value;
                    OnPropertChanged("ParcelWidth");
                }
            }
        }
        private string _parcelLength;
        public string ParcelLength
        {
            get { return _parcelLength ?? "1"; }
            set
            {
                if (value != _parcelLength)
                {
                    _parcelLength = value;
                    OnPropertChanged("ParcelLength");
                }
            }
        }
        private string _parcelWorth;
        public string ParcelWorth
        {
            get { return _parcelWorth ?? "1"; }
            set
            {
                if (value != _parcelWorth)
                {
                    _parcelWorth = value;
                    OnPropertChanged("ParcelWorth");
                }
            }
        }
        public double ParcelDistance { get; set; }
        public double ParcelDuration { get; set; }
        public Tariff MyTariff { get; set; }

        public BindableCollection<TypeOfParcel> ParcelType { get; set; }

        private TypeOfParcel _parcelTypeSelected;
        public TypeOfParcel ParcelTypeSelected 
        {
            get { return _parcelTypeSelected; }
            set
            {
                if(value != _parcelTypeSelected)
                {
                    _parcelTypeSelected = value;
                    OnPropertChanged("ParcelTypeSelected");
                }
            }
        }

        public BindableCollection<MethodOfSend> ParcelSendMethod { get; set; }

        private MethodOfSend _parcelSendMethodSelected;
        public MethodOfSend ParcelSendMethodSelected
        {
            get { return _parcelSendMethodSelected; }
            set
            {
                if (value != _parcelSendMethodSelected)
                {
                    _parcelSendMethodSelected = value;
                    OnPropertChanged("ParcelSendMethodSelected");
                }
            }
        }


        public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                string result = null;
                int NIP;
                bool condition;
                decimal value;
                switch (columnName)
                {
                    case "SenderFirstName":
                        if ((!string.IsNullOrEmpty(SenderCompanyName) && !string.IsNullOrEmpty(SenderCompanyNIP)) && string.IsNullOrWhiteSpace(SenderLastName))
                            break;
                        if (string.IsNullOrWhiteSpace(SenderFirstName))
                            result = "Pole nie może być puste";
                        else if (SenderFirstName.Length > 25)
                            result = "Długość imienia nie może przekraczać 25 znaków";
                        break;
                    case "SenderLastName":
                        if ((!string.IsNullOrEmpty(SenderCompanyName) && !string.IsNullOrEmpty(SenderCompanyNIP)) && string.IsNullOrWhiteSpace(SenderFirstName))
                            break;
                        if (string.IsNullOrWhiteSpace(SenderLastName))
                            result = "Pole nie może być puste";
                        else if (SenderLastName.Length > 30)
                            result = "Długość nazwiska nie może przekraczać 30 znaków";
                        break;
                    case "SenderCompanyName":
                        if ((!string.IsNullOrEmpty(SenderFirstName) && !string.IsNullOrEmpty(SenderLastName)) && string.IsNullOrWhiteSpace(SenderCompanyNIP))
                            break;
                        if (string.IsNullOrWhiteSpace(SenderCompanyName))
                            result = "Pole nie może być puste";
                        else if (SenderCompanyName.Length > 25)
                            result = "Nazwa firmy nie może przekraczać 25 znaków";
                        break;
                    case "SenderCompanyNIP":
                        if ((!string.IsNullOrEmpty(SenderFirstName) && !string.IsNullOrEmpty(SenderLastName)) && string.IsNullOrWhiteSpace(SenderCompanyName))
                            break;
                        if (string.IsNullOrWhiteSpace(SenderCompanyNIP))
                            result = "Pole nie może być puste";
                        else
                        if (decimal.TryParse(SenderCompanyNIP, out value))
                        {
                        if (value <= 0)
                            result = "Numer NIP jest wymagany";
                        }
                        else
                        {
                            result = "Podana wartość nie jest liczbą";
                        }
                        break;
                    case "SenderTel":
                        condition = int.TryParse(SenderTel, out NIP);
                        if (string.IsNullOrWhiteSpace(SenderTel))
                            result = "Pole nie może być puste";
                        else if (!condition)
                            result = "Numer telefonu musi zawierać wyłącznie cyfry";
                        else if (SenderTel.Length > 30)
                            result = "Długość numeru telefonu nie może przekraczać 12 znaków";
                        break;
                    case "SenderCountry":
                        if (string.IsNullOrWhiteSpace(SenderCountry))
                            result = "Pole nie może być puste";
                        else if (SenderCountry.Length > 20)
                            result = "Długość nazwy kraju nie może przekraczać 20 znaków";
                        break;
                    case "SenderCity":
                        if (string.IsNullOrWhiteSpace(SenderCity))
                            result = "Pole nie może być puste";
                        else if (SenderCity.Length > 30)
                            result = "Długość nazwy miasta nie może przekraczać 30 znaków";
                        break;
                    case "SenderPostalCode":
                        if (string.IsNullOrWhiteSpace(SenderPostalCode))
                            result = "Pole nie może być puste";
                        else if (SenderPostalCode.Length > 10)
                            result = "Długość kodu pocztowego nie może przekraczać 10 znaków";
                        break;
                    case "SenderStreet":
                        if (string.IsNullOrWhiteSpace(SenderStreet))
                            break;
                        if (SenderStreet.Length > 30)
                            result = "Długość nazwy ulicy nie może przekraczać 30 znaków";
                        break;
                    case "SenderNumberOfHouse":
                        if (string.IsNullOrWhiteSpace(SenderNumberOfHouse))
                            result = "Pole nie może być puste";
                        else if (SenderNumberOfHouse.Length > 10)
                            result = "Długość numeru domu nie może przekraczać 10 znaków";
                        break;
                    case "ReceiverFirstName":
                        if ((!string.IsNullOrEmpty(ReceiverCompanyName) && !string.IsNullOrEmpty(ReceiverCompanyNIP)) && string.IsNullOrWhiteSpace(ReceiverFirstName))
                            break;
                        if (string.IsNullOrWhiteSpace(ReceiverFirstName))
                            result = "Pole nie może być puste";
                        else if (ReceiverFirstName.Length > 25)
                            result = "Długość imienia nie może przekraczać 25 znaków";
                        break;
                    case "ReceiverLastName":
                        if ((!string.IsNullOrEmpty(ReceiverCompanyName) && !string.IsNullOrEmpty(ReceiverCompanyNIP)) && string.IsNullOrWhiteSpace(ReceiverLastName))
                            break;
                        if (string.IsNullOrWhiteSpace(ReceiverLastName))
                            result = "Pole nie może być puste";
                        else if (ReceiverLastName.Length > 30)
                            result = "Długość nazwiska nie może przekraczać 30 znaków";
                        break;
                    case "ReceiverCompanyName":
                        if ((!string.IsNullOrEmpty(ReceiverFirstName) && !string.IsNullOrEmpty(ReceiverLastName)) && string.IsNullOrWhiteSpace(ReceiverCompanyNIP))
                            break;
                        if (string.IsNullOrWhiteSpace(ReceiverCompanyName))
                            result = "Pole nie może być puste";
                        else if (ReceiverCompanyName.Length > 25)
                            result = "Nazwa firmy nie może przekraczać 25 znaków";
                        break;
                    case "ReceiverCompanyNIP":
                        if ((!string.IsNullOrEmpty(ReceiverFirstName) && !string.IsNullOrEmpty(ReceiverLastName)) && string.IsNullOrWhiteSpace(ReceiverCompanyName))
                            break;
                        if(string.IsNullOrWhiteSpace(ReceiverCompanyName))
                            result = "Pole nie może być puste";
                        else
                        if (decimal.TryParse(ReceiverCompanyNIP, out value))
                        {
                            if (value <= 0)
                                result = "Numer NIP jest wymagany";
                        }
                        else
                        {
                            result = "Podana wartość nie jest liczbą";
                        }
                        break;
                    case "ReceiverTel":
                        condition = int.TryParse(ReceiverTel, out NIP);
                        if (string.IsNullOrWhiteSpace(ReceiverTel))
                            result = "Pole nie może być puste";
                        else if (!condition)
                            result = "Numer telefonu musi zawierać wyłącznie cyfry";
                        else if (SenderTel?.Length > 30)
                            result = "Długość numeru telefonu nie może przekraczać 12 znaków";
                        break;
                    case "ReceiverCountry":
                        if (string.IsNullOrWhiteSpace(ReceiverCountry))
                            result = "Pole nie może być puste";
                        else if (ReceiverCountry.Length > 20)
                            result = "Długość nazwy kraju nie może przekraczać 20 znaków";
                        break;
                    case "ReceiverCity":
                        if (string.IsNullOrWhiteSpace(ReceiverCity))
                            result = "Pole nie może być puste";
                        else if (ReceiverCity.Length > 30)
                            result = "Długość nazwy miasta nie może przekraczać 30 znaków";
                        break;
                    case "ReceiverPostalCode":
                        if (string.IsNullOrWhiteSpace(ReceiverPostalCode))
                            result = "Pole nie może być puste";
                        else if (ReceiverPostalCode.Length > 10)
                            result = "Długość kodu pocztowego nie może przekraczać 10 znaków";
                        break;
                    case "ReceiverStreet":
                        if (string.IsNullOrWhiteSpace(ReceiverStreet))
                            break;
                        if (ReceiverStreet.Length > 30)
                            result = "Długość nazwy ulicy nie może przekraczać 30 znaków";
                        break;
                    case "ReceiverNumberOfHouse":
                        if (string.IsNullOrWhiteSpace(ReceiverNumberOfHouse))
                            result = "Pole nie może być puste";
                        else if (ReceiverNumberOfHouse.Length > 10)
                            result = "Długość numeru domu nie może przekraczać 10 znaków";
                        break;
                    case "ParcelWeight":
                        if (decimal.TryParse(ParcelWeight, out value))
                        {
                            if (value <= 0)
                                result = "Waga musi być większa od zera";
                        }
                        else
                        {
                            result = "Podana wartość nie jest liczbą";
                        }
                        break;
                    case "ParcelHeight":
                        if (decimal.TryParse(ParcelHeight, out value))
                        {
                            if (value <= 0)
                                result = "Waga musi być większa od zera";
                        }
                        else
                        {
                            result = "Podana wartość nie jest liczbą";
                        }
                        break;
                    case "ParcelWidth":
                        if (decimal.TryParse(ParcelWidth, out value))
                        {
                            if (value <= 0)
                                result = "Waga musi być większa od zera";
                        }
                        else
                        {
                            result = "Podana wartość nie jest liczbą";
                        }
                        break;
                    case "ParcelLength":
                        if (decimal.TryParse(ParcelLength, out value))
                        {
                            if (value <= 0)
                                result = "Waga musi być większa od zera";
                        }
                        else
                        {
                            result = "Podana wartość nie jest liczbą";
                        }
                        break;
                    case "ParcelWorth":
                        if (decimal.TryParse(ParcelWorth, out value))
                        {
                            if (value <= 0)
                                result = "Waga musi być większa od zera";
                        }
                        else
                        {
                            result = "Podana wartość nie jest liczbą";
                        }
                        break;
                }
                return result;
            }
        }


        public ParcelAddViewModel()
        {
            ParcelType = new BindableCollection<TypeOfParcel>(CompanyEntities.TypeOfParcel.ToList());
            ParcelTypeSelected = ParcelType.FirstOrDefault();

            ParcelSendMethod = new BindableCollection<MethodOfSend>(CompanyEntities.MethodOfSend.ToList());
            ParcelSendMethodSelected = ParcelSendMethod.FirstOrDefault();
        }

        public bool SendParcel(DataModel.Region senderRegion, DataModel.Region receiverRegion, Location senderLocation, Location receiverLocation, bool generateLabel, bool generateConfirmation)
        {
            try
            {
                Customer sender = new Customer()
                {
                    firstName = SenderFirstName ?? "",
                    lastName = SenderLastName ?? "",
                    tel = int.Parse(SenderTel)
                };
                Customer receiver = new Customer()
                {
                    firstName = ReceiverFirstName ?? "",
                    lastName = ReceiverLastName ?? "",
                    tel = int.Parse(ReceiverTel)
                };
                if (!string.IsNullOrWhiteSpace(SenderCompanyNIP))
                {
                    Company senderCompany = new Company()
                    {
                        name = SenderCompanyName,
                        NIP = int.Parse(SenderCompanyNIP)
                    };
                    CompanyEntities.Company.Add(senderCompany);
                    CompanyEntities.SaveChanges();
                    sender.Company = senderCompany;
                    sender.idCompany = senderCompany.id;
                }
                if (!string.IsNullOrWhiteSpace(ReceiverCompanyNIP))
                {
                    Company receiverCompany = new Company()
                    {
                        name = ReceiverCompanyName,
                        NIP = int.Parse(ReceiverCompanyNIP)
                    };
                    CompanyEntities.Company.Add(receiverCompany);
                    CompanyEntities.SaveChanges();
                    receiver.Company = receiverCompany;
                    receiver.idCompany = receiverCompany.id;
                }
                Address senderAddress = new Address()
                {
                    country = SenderCountry ?? "",
                    city = SenderCity ?? "",
                    postalCode = SenderPostalCode ?? "",
                    street = SenderStreet ?? "",
                    numberOfHouse = SenderNumberOfHouse ?? ""
                };
                Address receiverAddress = new Address()
                {
                    country = ReceiverCountry ?? "",
                    city = ReceiverCity ?? "",
                    postalCode = ReceiverPostalCode ?? "",
                    street = ReceiverStreet ?? "",
                    numberOfHouse = ReceiverNumberOfHouse ?? ""
                };
                var senderLocalization = new DataModel.Localization()
                {
                    latitude = senderLocation.Latitude.ToString(),
                    longitude = senderLocation.Longitude.ToString()
                };
                var receiverLocalization = new DataModel.Localization()
                {
                    latitude = receiverLocation.Latitude.ToString(),
                    longitude = receiverLocation.Longitude.ToString()
                };
                CompanyEntities.Localization.Add(senderLocalization);
                CompanyEntities.Localization.Add(receiverLocalization);
                CompanyEntities.SaveChanges();
                senderAddress.Localization = senderLocalization;
                senderAddress.idLocalization = senderLocalization.id;
                receiverAddress.Localization = receiverLocalization;
                receiverAddress.idLocalization = receiverLocalization.id;
                CompanyEntities.Address.Add(senderAddress);
                CompanyEntities.Address.Add(receiverAddress);
                CompanyEntities.SaveChanges();
                sender.Address = senderAddress;
                sender.idAddress = senderAddress.id;
                receiver.Address = receiverAddress;
                receiver.idAddress = receiverAddress.id;

                CompanyEntities.Customer.Add(sender);
                CompanyEntities.Customer.Add(receiver);
                CompanyEntities.SaveChanges();

                Parcel parcel = new Parcel()
                {
                    idSender = sender.id,
                    idReceiver = receiver.id,
                    idReceiverRegion = receiverRegion.id,
                    height = (decimal)double.Parse(ParcelHeight),
                    width = (decimal)double.Parse(ParcelWidth),
                    length = (decimal)double.Parse(ParcelLength),
                    amount = (decimal)double.Parse(ParcelWorth),
                    weight = (decimal)double.Parse(ParcelWeight),
                    idTariff = MyTariff.id,
                    idTypeOfParcel = ParcelTypeSelected.id,
                    idMethodOfSend = ParcelSendMethodSelected.id,
                    dateOfShipment = DateTime.Now,
                    idStatus = (int)EnumParcelStatus.registered
                };
                if (senderRegion != null)
                {
                    parcel.idSenderRegion = senderRegion.id;
                    parcel.code = $"{senderRegion.Warehouse.code}/{receiverRegion.Warehouse.code}/{DateTime.Now.Year - 2000}";
                }
                else
                    parcel.code = $"{receiverRegion.Warehouse.code}/{DateTime.Now.Year - 2000}";
                CompanyEntities.Parcel.Add(parcel);
                CompanyEntities.SaveChanges();
                parcel.code = $"{parcel.id}/{parcel.code}";
                CompanyEntities.SaveChanges();

                if (generateLabel)
                    GenerateLabel(parcel.id);
                if (generateConfirmation)
                    GenerateConfirmation(parcel.id);
                return true;
            }catch(Exception e)
            {
                return false;
            }
        }

        public void GenerateLabel(int parcelId)
        {
            Parcel parcel = CompanyEntities.Parcel.Include(p => p.Region)
                .Include(p => p.Region1)
                .Include(p => p.Tariff)
                .Include(p => p.Customer)
                .Include(p => p.Customer.Address)
                .Include(p => p.Customer.Company)
                .Include(p => p.Customer1)
                .Include(p => p.Customer1.Address)
                .Include(p => p.Customer1.Company)
                .FirstOrDefault(p => p.id == parcelId);
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Verdana", 16, XFontStyle.Bold);
            XFont fontDimension = new XFont("Verdana", 12, XFontStyle.Bold);
            XFont fontData = new XFont("Verdana", 12, XFontStyle.Regular);
            XFont fontLight = new XFont("Verdana", 8, XFontStyle.Regular);


            gfx.DrawRectangle(XPens.Black, 10, 10, page.Width - 20, 300);
            gfx.DrawRectangle(XPens.Black, 10, 10, 250, 200);
            gfx.DrawRectangle(XPens.Black, 10, 10, 250, 60);
            gfx.DrawString("NADAWCA", font, XBrushes.Black, new XRect(10, 70, 125, 30), XStringFormat.Center);
            gfx.DrawRectangle(XPens.Black, 10, 70, 125, 30);
            gfx.DrawString("ODBIORCA", font, XBrushes.Black, new XRect(135, 70, 125, 30), XStringFormat.Center);
            gfx.DrawRectangle(XPens.Black, 135, 70, 125, 30);
            gfx.DrawRectangle(XPens.Black, 10, 100, 125, 110);
            gfx.DrawRectangle(XPens.Black, 135, 100, 125, 110);
            gfx.DrawRectangle(XPens.Black, 10, 210, 260, 25);
            gfx.DrawString("Wys:", fontDimension, XBrushes.Black, new XRect(10, 210, 100, 25), XStringFormat.Center);
            gfx.DrawRectangle(XPens.Black, 10, 210, 100, 25);
            gfx.DrawString($"{parcel.height} cm", fontData, XBrushes.Black, new XRect(110, 210, 160, 25), XStringFormat.Center);
            gfx.DrawRectangle(XPens.Black, 10, 235, 260, 25);
            gfx.DrawString("Szer:", fontDimension, XBrushes.Black, new XRect(10, 235, 100, 25), XStringFormat.Center);
            gfx.DrawRectangle(XPens.Black, 10, 235, 100, 25);
            gfx.DrawString($"{parcel.width} cm", fontData, XBrushes.Black, new XRect(110, 235, 160, 25), XStringFormat.Center);
            gfx.DrawRectangle(XPens.Black, 10, 260, 260, 25);
            gfx.DrawString("Dług:", fontDimension, XBrushes.Black, new XRect(10, 260, 100, 25), XStringFormat.Center);
            gfx.DrawRectangle(XPens.Black, 10, 260, 100, 25);
            gfx.DrawString($"{parcel.length} cm", fontData, XBrushes.Black, new XRect(110, 260, 160, 25), XStringFormat.Center);
            gfx.DrawRectangle(XPens.Black, 10, 285, 260, 25);
            gfx.DrawString("Waga:", fontDimension, XBrushes.Black, new XRect(10, 285, 100, 25), XStringFormat.Center);
            gfx.DrawRectangle(XPens.Black, 10, 285, 100, 25);
            gfx.DrawString($"{parcel.weight} kg", fontData, XBrushes.Black, new XRect(110, 285, 160, 25), XStringFormat.Center);
            gfx.DrawRectangle(XPens.Black, 10, 285, 260, 25);
            gfx.DrawString("Magazyn", fontLight, XBrushes.Black, new XRect(page.Width - 150, 10, 140, 30), XStringFormat.TopLeft);
            gfx.DrawString($"{parcel.Region.Warehouse.code}", fontDimension, XBrushes.Black, new XRect(page.Width - 150, 10, 140, 30), XStringFormat.Center);
            gfx.DrawRectangle(XPens.Black, page.Width - 150, 10, 140, 30);
            gfx.DrawString("Region", fontLight, XBrushes.Black, new XRect(page.Width - 150, 40, 140, 30), XStringFormat.TopLeft);
            gfx.DrawString($"{parcel.Region.code}", fontDimension, XBrushes.Black, new XRect(page.Width - 150, 40, 140, 30), XStringFormat.Center);
            gfx.DrawRectangle(XPens.Black, page.Width - 150, 40, 140, 30);
            gfx.DrawString("Kod paczki", fontLight, XBrushes.Black, new XRect(page.Width - 150, 70, 140, 30), XStringFormat.TopLeft);
            gfx.DrawString($"{parcel.code}", fontDimension, XBrushes.Black, new XRect(page.Width - 150, 70, 140, 30), XStringFormat.Center);
            gfx.DrawRectangle(XPens.Black, page.Width - 150, 70, 140, 30);
            gfx.DrawString("Data nadania", fontLight, XBrushes.Black, new XRect(270, 210, 140, 25), XStringFormat.TopLeft);
            gfx.DrawString($"{parcel.dateOfShipment.ToString("yyyy-MM-dd")}", fontDimension, XBrushes.Black, new XRect(270, 210, 140, 25), XStringFormat.BottomCenter);
            gfx.DrawRectangle(XPens.Black, 270, 210, 140, 25);
            gfx.DrawString("Pobranie", fontData, XBrushes.Black, new XRect(300, 235, 110, 25), XStringFormat.Center);
            gfx.DrawRectangle(XPens.Black, 270, 235, 140, 25);
            gfx.DrawRectangle(XPens.Black, 270, 235, 30, 25);
            gfx.DrawRectangle(XPens.Black, 275, 240, 15, 15);
            if (parcel.idTypeOfParcel == (int)EnumTypeOfParcel.CashOnDelivery)
            {
                gfx.DrawString("X", fontDimension, XBrushes.Black, new XRect(275, 240, 15, 15), XStringFormat.Center);
                gfx.DrawString($"{parcel.amount} zł", fontDimension, XBrushes.Black, new XRect(270, 260, 140, 50), XStringFormat.Center);
            }
            gfx.DrawRectangle(XPens.Black, 270, 260, 140, 50);
            gfx.DrawRectangle(XPens.Black, 410, 180, 175, 110);
            gfx.DrawString($"{parcel.id}", fontDimension, XBrushes.Black, new XRect(410, 290, 175, 20), XStringFormat.Center);

            if (!String.IsNullOrWhiteSpace(parcel.Customer.firstName))
                gfx.DrawString($"{parcel.Customer.firstName}", fontData, XBrushes.Black, new XPoint() { X = 20, Y = 120 });
            if (!String.IsNullOrWhiteSpace(parcel.Customer.lastName))
                gfx.DrawString($"{parcel.Customer.lastName}", fontData, XBrushes.Black, new XPoint() { X = 20, Y = 145 });
            if(parcel.Customer.Company != null && !String.IsNullOrWhiteSpace(parcel.Customer.Company.name))
                gfx.DrawString($"{parcel.Customer.Company.name}", fontData, XBrushes.Black, new XPoint() { X = 20, Y = 170});
            if (parcel.Customer.Company != null &&  parcel.Customer.Company.NIP != 0)
                gfx.DrawString($"{parcel.Customer.Company.NIP}", fontData, XBrushes.Black, new XPoint() { X = 20, Y = 195});

            if(!String.IsNullOrWhiteSpace(parcel.Customer1.firstName))
                gfx.DrawString($"{parcel.Customer1.firstName}", fontData, XBrushes.Black, new XPoint() { X = 145, Y = 120});
            if (!String.IsNullOrWhiteSpace(parcel.Customer1.lastName))
                gfx.DrawString($"{parcel.Customer1.lastName}", fontData, XBrushes.Black, new XPoint() { X = 145, Y = 145});
            if (parcel.Customer1.Company != null && !String.IsNullOrWhiteSpace(parcel.Customer1.Company.name))
                gfx.DrawString($"{parcel.Customer1.Company.name}", fontData, XBrushes.Black, new XPoint() { X = 145, Y = 170});
            if (parcel.Customer1.Company != null && parcel.Customer1.Company.NIP != 0)
                gfx.DrawString($"{parcel.Customer1.Company.NIP}", fontData, XBrushes.Black, new XPoint() { X = 145, Y = 195});

            gfx.DrawString($"{parcel.Customer1.Address.country}", fontData, XBrushes.Black, new XPoint() { X = 270, Y = 40});
            gfx.DrawString($"{parcel.Customer1.Address.postalCode}  {parcel.Customer1.Address.city}", fontData, XBrushes.Black, new XPoint() { X = 270, Y = 90});
            if(!String.IsNullOrWhiteSpace(parcel.Customer1.Address.street))
                gfx.DrawString($"ul.{parcel.Customer1.Address.street} {parcel.Customer1.Address.numberOfHouse}", fontData, XBrushes.Black, new XPoint() { X = 270, Y = 140});
            else
                gfx.DrawString($"{parcel.Customer1.Address.numberOfHouse}", fontData, XBrushes.Black, new XPoint() { X = 270, Y = 140});

            var QCwriter = new BarcodeWriter();
            QCwriter.Format = BarcodeFormat.QR_CODE;
            QCwriter.Options.Width = 210;
            QCwriter.Options.Height = 130;
            QCwriter.Options.Margin = 0;
            var result = QCwriter.Write(parcel.id.ToString());
            var barcodeBitmap = new Bitmap(result);
            using (MemoryStream memory = new MemoryStream())
            {
                barcodeBitmap.Save(memory, ImageFormat.Jpeg);
                XImage image = XImage.FromStream(memory);
                gfx.DrawImage(image, new XPoint() { X = 420, Y = 185 });
            }
            document.Save($"{parcel.id}Label.pdf");
            Process.Start($"{parcel.id}Label.pdf");
        }

        private void GenerateConfirmation(int parcelId)
        {
            Parcel parcel = CompanyEntities.Parcel.Include(p => p.Region)
                .Include(p => p.Region1)
                .Include(p => p.Tariff)
                .Include(p => p.Customer1)
                .Include(p => p.Customer1.Address)
                .Include(p => p.Customer1.Company)
                .FirstOrDefault(p => p.id == parcelId);
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Verdana", 16, XFontStyle.Bold);
            XFont fontDimension = new XFont("Verdana", 12, XFontStyle.Bold);
            XFont fontData = new XFont("Verdana", 12, XFontStyle.Regular);
            XFont fontLight = new XFont("Verdana", 8, XFontStyle.Regular);

            gfx.DrawString("Potwierdzenie nadania przesyłki", font, XBrushes.Black, new XPoint() { X = 150, Y = 80 });
            gfx.DrawString($"Data wystawienia: {DateTime.Now.ToString("yyyy-MM-dd")}", fontLight, XBrushes.Black, new XPoint() { X = page.Width - 150, Y = 30 });
            gfx.DrawString("Sprzedawca", fontDimension, XBrushes.Black, new XPoint() { X = 30, Y = 150 });
            gfx.DrawString("SuperKurier", fontData, XBrushes.Black, new XPoint() { X = 30, Y = 180 });
            gfx.DrawString("ul.Sienkiewicza 177", fontData, XBrushes.Black, new XPoint() { X = 30, Y = 230 });
            gfx.DrawString("08-110 Siedlce", fontData, XBrushes.Black, new XPoint() { X = 30, Y = 210 });
            gfx.DrawString("NIP: 821-222-11-99", fontData, XBrushes.Black, new XPoint() { X = 30, Y = 250 });
            gfx.DrawString("Nabywca", fontDimension, XBrushes.Black, new XPoint() { X = 320, Y = 150 });
            if(parcel.Customer.Company != null && !string.IsNullOrWhiteSpace(parcel.Customer.Company.name))
                gfx.DrawString(parcel.Customer.Company.name, fontData, XBrushes.Black, new XPoint() { X = 320, Y = 170 });
            if(parcel.Customer.firstName != null && !string.IsNullOrWhiteSpace(parcel.Customer.firstName))
                gfx.DrawString($"{parcel.Customer.firstName}  {parcel.Customer.lastName}", fontData, XBrushes.Black, new XPoint() { X = 320, Y = 190 });
            if (!string.IsNullOrWhiteSpace(parcel.Customer.Address.street))
            gfx.DrawString($"ul.{parcel.Customer.Address.street} {parcel.Customer.Address.numberOfHouse}", fontData, XBrushes.Black, new XPoint() { X = 320, Y = 230 });
            else
                gfx.DrawString($"{parcel.Customer.Address.numberOfHouse}", fontData, XBrushes.Black, new XPoint() { X = 320, Y = 230 });
            gfx.DrawString($"{parcel.Customer.Address.postalCode} {parcel.Customer.Address.city}", fontData, XBrushes.Black, new XPoint() { X = 320, Y = 210 });
            if (parcel.Customer.Company != null)
                gfx.DrawString($"NIP: {parcel.Customer.Company.NIP}", fontData, XBrushes.Black, new XPoint() { X = 320, Y = 250 });
            gfx.DrawString("ID przesyłki", fontDimension, XBrushes.Black, new XPoint() { X = 50, Y = 300 });
            gfx.DrawString("Data nadania", fontDimension, XBrushes.Black, new XPoint() { X = 250, Y = 300 });
            gfx.DrawString("Koszt", fontDimension, XBrushes.Black, new XPoint() { X = 450, Y = 300 });
            gfx.DrawString(parcel.id.ToString(), fontData, XBrushes.Black, new XPoint() { X = 50, Y = 330 });
            gfx.DrawString(parcel.dateOfShipment.ToString("yyyy-MM-dd"), fontData, XBrushes.Black, new XPoint() { X = 250, Y = 330 });
            if(parcel.idTypeOfParcel == (int)EnumTypeOfParcel.CashOnDelivery)
             gfx.DrawString($"{parcel.Tariff.cost + 10} zł", fontData, XBrushes.Black, new XPoint() { X = 450, Y = 330 });
            else
            {
             gfx.DrawString($"{parcel.Tariff.cost} zł", fontData, XBrushes.Black, new XPoint() { X = 450, Y = 330 });
            }
            gfx.DrawString("--------------------------------------------------------------------", fontLight, XBrushes.Black, new XPoint() { X = 300, Y = 480 });
            gfx.DrawString("Podpis osoby uprawionej do wystawienia potwierdzenia", fontLight, XBrushes.Black, new XPoint() { X = 310, Y = 485 });

            document.Save($"{parcel.id}Confirmation.pdf");
            Process.Start($"{parcel.id}Confirmation.pdf");
        }
    }
}
