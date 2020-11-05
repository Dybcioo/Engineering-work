using Caliburn.Micro;
using DataModel;
using Microsoft.Maps.MapControl.WPF;
using SuperKurier.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SuperKurier.ViewModel
{
    class ParcelAddViewModel : ParcelViewModel, IDataErrorInfo
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
                    OnPropertChanged("SenderCompanyName");
                    OnPropertChanged("SenderLastName");
                    OnPropertChanged("SenderFirstName");
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
                    OnPropertChanged("SenderCompanyNIP");
                    OnPropertChanged("SenderLastName");
                    OnPropertChanged("SenderFirstName");
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
                    OnPropertChanged("ReceiverCompanyName");
                    OnPropertChanged("ReceiverFirstName");
                    OnPropertChanged("ReceiverLastName");
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
                    OnPropertChanged("ReceiverCompanyNIP");
                    OnPropertChanged("ReceiverFirstName");
                    OnPropertChanged("ReceiverLastName");
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
                        if (!string.IsNullOrEmpty(SenderCompanyName) || !string.IsNullOrEmpty(SenderCompanyNIP))
                            break;
                        if (string.IsNullOrWhiteSpace(SenderFirstName))
                            result = "Pole nie może być puste";
                        else if (SenderFirstName.Length > 25)
                            result = "Długość imienia nie może przekraczać 25 znaków";
                        break;
                    case "SenderLastName":
                        if (!string.IsNullOrEmpty(SenderCompanyName) || !string.IsNullOrEmpty(SenderCompanyNIP))
                            break;
                        if (string.IsNullOrWhiteSpace(SenderLastName))
                            result = "Pole nie może być puste";
                        else if (SenderLastName.Length > 30)
                            result = "Długość nazwiska nie może przekraczać 30 znaków";
                        break;
                    case "SenderCompanyName":
                        if (!string.IsNullOrEmpty(SenderFirstName) || !string.IsNullOrEmpty(SenderLastName))
                            break;
                        if (string.IsNullOrWhiteSpace(SenderCompanyName))
                            result = "Pole nie może być puste";
                        else if (SenderCompanyName.Length > 25)
                            result = "Nazwa firmy nie może przekraczać 25 znaków";
                        break;
                    case "SenderCompanyNIP":
                        condition = int.TryParse(SenderCompanyNIP, out NIP);
                        if (!string.IsNullOrEmpty(SenderFirstName) || !string.IsNullOrEmpty(SenderLastName))
                            break;
                        if (string.IsNullOrEmpty(SenderCompanyNIP))
                            result = "Pole nie może być puste";
                        else if (!condition)
                            result = "NIP musi zawierać wyłącznie cyfry";
                        else if (SenderCompanyNIP.Length != 10)
                            result = "NIP musi zawierać 10 cyfr";
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
                        if (!string.IsNullOrEmpty(ReceiverCompanyName) || !string.IsNullOrEmpty(ReceiverCompanyNIP))
                            break;
                        if (string.IsNullOrWhiteSpace(ReceiverFirstName))
                            result = "Pole nie może być puste";
                        else if (ReceiverFirstName.Length > 25)
                            result = "Długość imienia nie może przekraczać 25 znaków";
                        break;
                    case "ReceiverLastName":
                        if (!string.IsNullOrEmpty(ReceiverCompanyName) || !string.IsNullOrEmpty(ReceiverCompanyNIP))
                            break;
                        if (string.IsNullOrWhiteSpace(ReceiverLastName))
                            result = "Pole nie może być puste";
                        else if (ReceiverLastName.Length > 30)
                            result = "Długość nazwiska nie może przekraczać 30 znaków";
                        break;
                    case "ReceiverCompanyName":
                        if (!string.IsNullOrEmpty(ReceiverFirstName) || !string.IsNullOrEmpty(ReceiverLastName))
                            break;
                        if (string.IsNullOrWhiteSpace(ReceiverCompanyName))
                            result = "Pole nie może być puste";
                        else if (ReceiverCompanyName.Length > 25)
                            result = "Nazwa firmy nie może przekraczać 25 znaków";
                        break;
                    case "ReceiverCompanyNIP":
                         condition = int.TryParse(ReceiverCompanyNIP, out NIP);
                        if (!string.IsNullOrEmpty(ReceiverFirstName) || !string.IsNullOrEmpty(ReceiverLastName))
                            break;
                        if (string.IsNullOrEmpty(ReceiverCompanyNIP))
                            result = "Pole nie może być puste";
                        else if (!condition)
                            result = "NIP musi zawierać wyłącznie cyfry";
                        else if (ReceiverCompanyNIP.Length != 10)
                            result = "NIP musi zawierać 10 cyfr";
                        break;
                    case "ReceiverTel":
                        condition = int.TryParse(ReceiverTel, out NIP);
                        if (string.IsNullOrWhiteSpace(ReceiverTel))
                            result = "Pole nie może być puste";
                        else if (!condition)
                            result = "Numer telefonu musi zawierać wyłącznie cyfry";
                        else if (SenderTel.Length > 30)
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

        public bool SendParcel(DataModel.Region senderRegion, DataModel.Region receiverRegion, Location senderLocation, Location receiverLocation)
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
                    idStatus = (int)enumParcelStatus.registered
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
                return true;
            }catch(Exception e)
            {
                return false;
            }
        }
    }
}
