using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

using ComponentSpace.SAML2;
using ComponentSpace.SAML2.Assertions;
using ComponentSpace.SAML2.Profiles.SSOBrowser;
using ComponentSpace.SAML2.Protocols;

using Selerix.BusinessObjects;
using Selerix.Foundation.Data;
using System.IO;

namespace BSTIntegrationExample
{
    /// <summary>
    /// State
    /// </summary>
    public enum STATE
    {
        /// <summary>
        /// Unknown
        /// </summary>
        UNKNOWN = 0,
        /// <summary>
        /// Alabama
        /// </summary>
        [Description("Alabama")]
        USA_AL = 1,
        /// <summary>
        /// Alaska
        /// </summary>
        [Description("Alaska")]
        USA_AK = 2,
        /// <summary>
        /// American Samoa
        /// </summary>
        [Description("American Samoa")]
        USA_AS = 3,
        /// <summary>
        /// Arizona
        /// </summary>
        [Description("Arizona")]
        USA_AZ = 4,
        /// <summary>
        /// Arkansas
        /// </summary>
        [Description("Arkansas")]
        USA_AR = 5,
        /// <summary>
        /// California
        /// </summary>
        [Description("California")]
        USA_CA = 6,
        /// <summary>
        /// Colorado
        /// </summary>
        [Description("Colorado")]
        USA_CO = 7,
        /// <summary>
        /// Connecticut
        /// </summary>
        [Description("Connecticut")]
        USA_CT = 8,
        /// <summary>
        /// Delaware
        /// </summary>
        [Description("Delaware")]
        USA_DE = 9,
        /// <summary>
        /// District of Columbia
        /// </summary>
        [Description("District of Columbia")]
        USA_DC = 10,
        /// <summary>
        /// Federated States of Micronesia
        /// </summary>
        [Description("Federated States of Micronesia")]
        USA_FS = 11,
        /// <summary>
        /// Florida
        /// </summary>
        [Description("Florida")]
        USA_FL = 12,
        /// <summary>
        /// Georgia
        /// </summary>
        [Description("Georgia")]
        USA_GA = 13,
        /// <summary>
        /// Guam
        /// </summary>
        [Description("Guam")]
        USA_GU = 14,
        /// <summary>
        /// Hawaii
        /// </summary>
        [Description("Hawaii")]
        USA_HI = 15,
        /// <summary>
        /// Idaho
        /// </summary>
        [Description("Idaho")]
        USA_ID = 16,
        /// <summary>
        /// Illinois
        /// </summary>
        [Description("Illinois")]
        USA_IL = 17,
        /// <summary>
        /// Indiana
        /// </summary>
        [Description("Indiana")]
        USA_IN = 18,
        /// <summary>
        /// Iowa
        /// </summary>
        [Description("Iowa")]
        USA_IA = 19,
        /// <summary>
        /// Kansas
        /// </summary>
        [Description("Kansas")]
        USA_KS = 20,
        /// <summary>
        /// Kentucky
        /// </summary>
        [Description("Kentucky")]
        USA_KY = 21,
        /// <summary>
        /// Louisiana
        /// </summary>
        [Description("Louisiana")]
        USA_LA = 22,
        /// <summary>
        /// Maine
        /// </summary>
        [Description("Maine")]
        USA_ME = 23,
        /// <summary>
        /// Marshall Islands
        /// </summary>
        [Description("Marshall Islands")]
        USA_MH = 24,
        /// <summary>
        /// Maryland
        /// </summary>
        [Description("Maryland")]
        USA_MD = 25,
        /// <summary>
        /// Massachusetts
        /// </summary>
        [Description("Massachusetts")]
        USA_MA = 26,
        /// <summary>
        /// Michigan
        /// </summary>
        [Description("Michigan")]
        USA_MI = 27,
        /// <summary>
        /// Minnesota
        /// </summary>
        [Description("Minnesota")]
        USA_MN = 28,
        /// <summary>
        /// Mississippi
        /// </summary>
        [Description("Mississippi")]
        USA_MS = 29,
        /// <summary>
        /// Missouri
        /// </summary>
        [Description("Missouri")]
        USA_MO = 30,
        /// <summary>
        /// Montana
        /// </summary>
        [Description("Montana")]
        USA_MT = 31,
        /// <summary>
        /// Nebraska
        /// </summary>
        [Description("Nebraska")]
        USA_NE = 32,
        /// <summary>
        /// Nevada
        /// </summary>
        [Description("Nevada")]
        USA_NV = 33,
        /// <summary>
        /// New Hampshire
        /// </summary>
        [Description("New Hampshire")]
        USA_NH = 34,
        /// <summary>
        /// New Jersey
        /// </summary>
        [Description("New Jersey")]
        USA_NJ = 35,
        /// <summary>
        /// New Mexico
        /// </summary>
        [Description("New Mexico")]
        USA_NM = 36,
        /// <summary>
        /// New York
        /// </summary>
        [Description("New York")]
        USA_NY = 37,
        /// <summary>
        /// North Carolina
        /// </summary>
        [Description("North Carolina")]
        USA_NC = 38,
        /// <summary>
        /// North Dakota
        /// </summary>
        [Description("North Dakota")]
        USA_ND = 39,
        /// <summary>
        /// Northern Mariana Islands
        /// </summary>
        [Description("Northern Mariana Islands")]
        USA_MP = 40,
        /// <summary>
        /// Ohio
        /// </summary>
        [Description("Ohio")]
        USA_OH = 41,
        /// <summary>
        /// Oklahoma
        /// </summary>
        [Description("Oklahoma")]
        USA_OK = 42,
        /// <summary>
        /// Oregon
        /// </summary>
        [Description("Oregon")]
        USA_OR = 43,
        /// <summary>
        /// Palau Island
        /// </summary>
        [Description("Palau Island")]
        USA_PW = 44,
        /// <summary>
        /// Pennsylvania
        /// </summary>
        [Description("Pennsylvania")]
        USA_PA = 45,
        /// <summary>
        /// Puerto Rico
        /// </summary>
        [Description("Puerto Rico")]
        USA_PR = 46,
        /// <summary>
        /// Rhode Island
        /// </summary>
        [Description("Rhode Island")]
        USA_RI = 47,
        /// <summary>
        /// South Carolina
        /// </summary>
        [Description("South Carolina")]
        USA_SC = 48,
        /// <summary>
        /// South Dakota
        /// </summary>
        [Description("South Dakota")]
        USA_SD = 49,
        /// <summary>
        /// Tennessee
        /// </summary>
        [Description("Tennessee")]
        USA_TN = 50,
        /// <summary>
        /// Texas
        /// </summary>
        [Description("Texas")]
        USA_TX = 51,
        /// <summary>
        /// Utah
        /// </summary>
        [Description("Utah")]
        USA_UT = 52,
        /// <summary>
        /// Vermont
        /// </summary>
        [Description("Vermont")]
        USA_VT = 53,
        /// <summary>
        /// Virgin Islands
        /// </summary>
        [Description("Virgin Islands")]
        USA_VI = 54,
        /// <summary>
        /// Virginia
        /// </summary>
        [Description("Virginia")]
        USA_VA = 55,
        /// <summary>
        /// Washington
        /// </summary>
        [Description("Washington")]
        USA_WA = 56,
        /// <summary>
        /// West Virginia
        /// </summary>
        [Description("West Virginia")]
        USA_WV = 57,
        /// <summary>
        /// Wisconsin
        /// </summary>
        [Description("Wisconsin")]
        USA_WI = 58,
        /// <summary>
        /// Wyoming
        /// </summary>
        [Description("Wyoming")]
        USA_WY = 59,
        /// <summary>
        /// Armed Forces Americas (except Canada)
        /// </summary>
        [Description("Armed Forces Americas (except Canada)")]
        USA_AA = 60,
        /// <summary>
        /// Armed Forces Canada, Africa, Europe, Middle East
        /// </summary>
        [Description("Armed Forces Canada, Africa, Europe, Middle East")]
        USA_AE = 61,
        /// <summary>
        /// US Armed Forces Pacific
        /// </summary>
        [Description("US Armed Forces Pacific")]
        USA_AP = 62,
        /// <summary>
        /// Guantanamo Bay (US Naval Base) Cuba
        /// </summary>
        [Description("Guantanamo Bay (US Naval Base) Cuba")]
        USA_GB = 80,
        /// <summary>
        /// Alberta
        /// </summary>
        CAN_AB = 101,
        /// <summary>
        /// British Columbia
        /// </summary>
        CAN_BC = 102,
        /// <summary>
        /// Manitoba
        /// </summary>
        CAN_MB = 103,
        /// <summary>
        /// New Brunswick
        /// </summary>
        CAN_NB = 104,
        /// <summary>
        /// AKA NL Newfoundland and Labrador
        /// </summary>
        CAN_NL = 105,
        /// <summary>
        /// Northwest Territories
        /// </summary>
        CAN_NT = 106,
        /// <summary>
        /// Nova Scotia
        /// </summary>
        CAN_NS = 107,
        /// <summary>
        /// Ontario
        /// </summary>
        CAN_ON = 108,
        /// <summary>
        /// Prince Edward Island
        /// </summary>
        CAN_PE = 109,
        /// <summary>
        /// Quebec
        /// </summary>
        CAN_PQ = 110,
        /// <summary>
        /// Saskatchewan
        /// </summary>
        CAN_SK = 111,
        /// <summary>
        /// Yukon Territory
        /// </summary>
        CAN_YT = 112,
        /// <summary>
        /// Nunavut
        /// </summary>
        CAN_NUNAVUT = 113,
        /// <summary>
        /// Australian Capital Territory
        /// </summary>
        AUS_ACT = 201,
        /// <summary>
        /// New South Wales
        /// </summary>
        AUS_NSW = 202,
        /// <summary>
        /// Northern Territory
        /// </summary>
        AUS_NT = 203,
        /// <summary>
        /// Queensland
        /// </summary>
        AUS_QL = 204,
        /// <summary>
        /// South Australia
        /// </summary>
        AUS_SA = 205,
        /// <summary>
        /// Tasmania
        /// </summary>
        AUS_TAS = 206,
        /// <summary>
        /// Victoria
        /// </summary>
        AUS_VIC = 207,
        /// <summary>
        /// Western Australia
        /// </summary>
        AUS_WA = 208,
        /// <summary>
        /// Aichi
        /// </summary>
        JPN_AICHI = 301,
        /// <summary>
        /// Akita
        /// </summary>
        JPN_AKITA = 302,
        /// <summary>
        /// Aomori
        /// </summary>
        JPN_AOMORI = 303,
        /// <summary>
        /// Chiba
        /// </summary>
        JPN_CHIBA = 304,
        /// <summary>
        /// Ehime
        /// </summary>
        JPN_EHIME = 305,
        /// <summary>
        /// Fukui
        /// </summary>
        JPN_FUKUI = 306,
        /// <summary>
        /// Fukuoka
        /// </summary>
        JPN_FUKUOKA = 307,
        /// <summary>
        /// Fukushima
        /// </summary>
        JPN_FUKUSHIMA = 308,
        /// <summary>
        /// Gifu
        /// </summary>
        JPN_GIFU = 309,
        /// <summary>
        /// Gunma
        /// </summary>
        JPN_GUNMA = 310,
        /// <summary>
        /// Hiroshima
        /// </summary>
        JPN_HIROSHIMA = 311,
        /// <summary>
        /// Hokkaido
        /// </summary>
        JPN_HOKKAIDO = 312,
        /// <summary>
        /// Hyogo
        /// </summary>
        JPN_HYOGO = 313,
        /// <summary>
        /// Ibaraki
        /// </summary>
        JPN_IBARAKI = 314,
        /// <summary>
        /// Ishikawa
        /// </summary>
        JPN_ISHIKAWA = 315,
        /// <summary>
        /// Iwate
        /// </summary>
        JPN_IWATE = 316,
        /// <summary>
        /// Kagawa
        /// </summary>
        JPN_KAGAWA = 317,
        /// <summary>
        /// Kagoshima
        /// </summary>
        JPN_KAGOSHIMA = 318,
        /// <summary>
        /// Kanagawa
        /// </summary>
        JPN_KANAGAWA = 319,
        /// <summary>
        /// Kouchi
        /// </summary>
        JPN_KOUCHI = 320,
        /// <summary>
        /// Kumamoto
        /// </summary>
        JPN_KUMAMOTO = 321,
        /// <summary>
        /// Kyoto
        /// </summary>
        JPN_KYOTO = 322,
        /// <summary>
        /// Mie
        /// </summary>
        JPN_MIE = 323,
        /// <summary>
        /// Miyagi
        /// </summary>
        JPN_MIYAGI = 324,
        /// <summary>
        /// Miyazaki
        /// </summary>
        JPN_MIYAZAKI = 325,
        /// <summary>
        /// Nagano
        /// </summary>
        JPN_NAGANO = 326,
        /// <summary>
        /// Nagasaki
        /// </summary>
        JPN_NAGASAKI = 327,
        /// <summary>
        /// Nara
        /// </summary>
        JPN_NARA = 328,
        /// <summary>
        /// Niigata
        /// </summary>
        JPN_NIIGATA = 329,
        /// <summary>
        /// Oita
        /// </summary>
        JPN_OITA = 330,
        /// <summary>
        /// Okayama
        /// </summary>
        JPN_OKAYAMA = 331,
        /// <summary>
        /// Okinawa
        /// </summary>
        JPN_OKINAWA = 332,
        /// <summary>
        /// Osaka
        /// </summary>
        JPN_OSAKA = 333,
        /// <summary>
        /// Saga
        /// </summary>
        JPN_SAGA = 334,
        /// <summary>
        /// Saitama
        /// </summary>
        JPN_SAITAMA = 335,
        /// <summary>
        /// Shiga
        /// </summary>
        JPN_SHIGA = 336,
        /// <summary>
        /// Shimane
        /// </summary>
        JPN_SHIMANE = 337,
        /// <summary>
        /// Shizuoka
        /// </summary>
        JPN_SHIZUOKA = 338,
        /// <summary>
        /// Tochigi
        /// </summary>
        JPN_TOCHIGI = 339,
        /// <summary>
        /// Tokushima
        /// </summary>
        JPN_TOKUSHIMA = 340,
        /// <summary>
        ///  Tokyo
        /// </summary>
        JPN_TOKYO = 341,
        /// <summary>
        /// Tottori
        /// </summary>
        JPN_TOTTORI = 342,
        /// <summary>
        /// Toyama
        /// </summary>
        JPN_TOYAMA = 343,
        /// <summary>
        /// Wakayama
        /// </summary>
        JPN_WAKAYAMA = 344,
        /// <summary>
        /// Yamagata
        /// </summary>
        JPN_YAMAGATA = 345,
        /// <summary>
        /// Yamaguchi
        /// </summary>
        JPN_YAMAGUCHI = 346,
        /// <summary>
        /// Yamanashi
        /// </summary>
        JPN_YAMANASHI = 347,
        /// <summary>
        /// Aguascalientes
        /// </summary>
        MEX_AGS = 401,
        /// <summary>
        /// Baja California
        /// </summary>
        MEX_BC = 402,
        /// <summary>
        /// Baja California Sur
        /// </summary>
        MEX_BCS = 403,
        /// <summary>
        /// Campeche
        /// </summary>
        MEX_CAMP = 404,
        /// <summary>
        /// Chiapas
        /// </summary>
        MEX_CHIS = 405,
        /// <summary>
        /// Chihuahua
        /// </summary>
        MEX_CHIH = 406,
        /// <summary>
        /// Coahuila
        /// </summary>
        MEX_COAH = 407,
        /// <summary>
        /// Colima
        /// </summary>
        MEX_COL = 408,
        /// <summary>
        /// Distrito Federal
        /// </summary>
        MEX_DF = 409,
        /// <summary>
        /// Durango
        /// </summary>
        MEX_DGO = 410,
        /// <summary>
        /// Guanajuato
        /// </summary>
        MEX_GTO = 411,
        /// <summary>
        /// Guerrero
        /// </summary>
        MEX_GRO = 412,
        /// <summary>
        /// Hidalgo
        /// </summary>
        MEX_HGO = 413,
        /// <summary>
        /// Jalisco
        /// </summary>
        MEX_JAL = 414,
        /// <summary>
        /// México
        /// </summary>
        MEX_MEX = 415,
        /// <summary>
        /// Michoacan
        /// </summary>
        MEX_MICH = 416,
        /// <summary>
        /// Morelos
        /// </summary>
        MEX_MOR = 417,
        /// <summary>
        /// Nayarit
        /// </summary>
        MEX_NAY = 418,
        /// <summary>
        /// Nuevo León
        /// </summary>
        MEX_NL = 419,
        /// <summary>
        /// Oaxaca
        /// </summary>
        MEX_OAX = 420,
        /// <summary>
        /// Puebla
        /// </summary>
        MEX_PUE = 421,
        /// <summary>
        /// Querétaro
        /// </summary>
        MEX_QRO = 422,
        /// <summary>
        /// Quintana Roo
        /// </summary>
        MEX_QR = 423,
        /// <summary>
        /// San Luis Potos
        /// </summary>
        MEX_SLP = 424,
        /// <summary>
        /// Sinaloa
        /// </summary>
        MEX_SIN = 425,
        /// <summary>
        /// Sonora
        /// </summary>
        MEX_SON = 426,
        /// <summary>
        /// Tabasco
        /// </summary>
        MEX_TAB = 427,
        /// <summary>
        /// Tamaulipas
        /// </summary>
        MEX_TAMPS = 428,
        /// <summary>
        /// Tlaxcala
        /// </summary>
        MEX_TLAX = 429,
        /// <summary>
        /// Veracruz
        /// </summary>
        MEX_VER = 430,
        /// <summary>
        /// Yucatán
        /// </summary>
        MEX_YUC = 431,
        /// <summary>
        /// Zacatecas
        /// </summary>
        MEX_ZAC = 432,
        /// <summary>
        /// Gauteng
        /// </summary>
        ZA_GAUTENG = 501,
        /// <summary>
        /// Western Cape
        /// </summary>
        ZA_WESTERNCAPE = 502,
        /// <summary>
        /// Northern Province
        /// </summary>
        ZA_NORTHPROVINCE = 503,
        /// <summary>
        /// Northwest Province
        /// </summary>
        ZA_NWPROVINCE = 504,
        /// <summary>
        /// Kwa Zulu Natal
        /// </summary>
        ZA_KWZULUNATAL = 505,
        /// <summary>
        /// Eastern Cape
        /// </summary>
        ZA_EASTCAPE = 506,
        /// <summary>
        /// Freestate
        /// </summary>
        ZA_FREESTATE = 507,
        /// <summary>
        /// Northern Cape
        /// </summary>
        ZA_NORTHCAPE = 508,
        /// <summary>
        /// Mpumalanga
        /// </summary>
        ZA_MPUMALANGA = 509,
        /// <summary>
        /// All states
        /// </summary>
        ALL_STATES = 1000,
        /// <summary>
        /// Other
        /// </summary>
        OTHER = 2147483647,
    }
    
    public partial class Saml2BAAgentExample : System.Web.UI.Page
    {
        #region Fields

        private TextBox _EmailText;

        private TextBox _FirstName;
        private TextBox _MiddleInitial;
        private TextBox _LastName;
        private TextBox _Suffix;

        private TextBox _Phone1;
        private TextBox _Phone2;
        private TextBox _Phone3;
        private TextBox _Phone4;

        private TextBox _AddressLine1;
        private TextBox _AddressLine2;
        private TextBox _City;
        private DropDownList _State;
        private TextBox _Zip;
        private TextBox _XMLTransmittal;

        private System.Web.UI.WebControls.Table _Table;
        private System.Web.UI.WebControls.Table _TableName;
        private System.Web.UI.WebControls.Table _TableAddress;

        #endregion

        private void CreateControls()
        {
            this.gridPlaceHolder.Controls.Clear();
            BuildQuestions();
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            CreateControls();

            if (!Page.IsPostBack)
            {
                _Table.Rows[6].Attributes.Add("style", "display:none;");
            }
        }

        private void BuildQuestions()
        {
            _Table = BuildTable(9, 2, "clsQuestion");
            _Table.ID = "table";
            _Table.Width = Unit.Percentage(100);

            _Table.Rows[0].Cells[0].Width = Unit.Percentage(20);
            _Table.Rows[0].Cells[1].Width = Unit.Percentage(80);

            //Email START ---------------------------------------------------------------------------------------
            _Table.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Right;
            _Table.Rows[0].Cells[0].Text = "E-mail Address:";

            _EmailText = new TextBox();
            _EmailText.ID = "emailText";
            _EmailText.Width = Unit.Pixel(250);

            _Table.Rows[0].Cells[1].Controls.Add(_EmailText);
            //Email END -----------------------------------------------------------------------------------------

            //Name START ----------------------------------------------------------------------------------------
            _Table.Rows[1].Cells[0].HorizontalAlign = HorizontalAlign.Right;
            _Table.Rows[1].Cells[0].Style.Add("padding-top", "10px");
            _Table.Rows[1].Cells[0].Text = "Name:";

            _TableName = BuildTable(1, 4, "clsQuestion");
            _TableName.ID = "tableName";

            //First Name
            _FirstName = new TextBox();
            _FirstName.ID = "firstName";
            _FirstName.Width = Unit.Pixel(120);

            _TableName.Rows[0].Cells[0].Width = Unit.Percentage(10);
            _TableName.Rows[0].Cells[0].Controls.Add(_FirstName);

            //Middle Initial
            _MiddleInitial = new TextBox();
            _MiddleInitial.ID = "middleInitial";
            _MiddleInitial.Width = Unit.Pixel(50);

            _TableName.Rows[0].Cells[1].Width = Unit.Percentage(5);
            _TableName.Rows[0].Cells[1].Controls.Add(_MiddleInitial);

            //Last Name
            _LastName = new TextBox();
            _LastName.ID = "lastName";
            _LastName.Width = Unit.Pixel(120);

            _TableName.Rows[0].Cells[2].Width = Unit.Percentage(10);
            _TableName.Rows[0].Cells[2].Controls.Add(_LastName);

            //Suffix
            _Suffix = new TextBox();
            _Suffix.ID = "suffix";
            _Suffix.Width = Unit.Pixel(50);

            _TableName.Rows[0].Cells[3].Width = Unit.Percentage(60);
            _TableName.Rows[0].Cells[3].Controls.Add(_Suffix);

            //FirstName
            Label firstName = new Label();
            firstName.ID = "firstNameLabel";
            firstName.Text = "<br />First Name";
            firstName.CssClass = "clsCaption";

            _TableName.Rows[0].Cells[0].Controls.Add(firstName);

            //MiddleInitial
            Label middleInitial = new Label();
            middleInitial.ID = "middleInitialLabel";
            middleInitial.Text = "<br />MI";
            middleInitial.CssClass = "clsCaptionOpt";

            _TableName.Rows[0].Cells[1].Controls.Add(middleInitial);

            //LastName
            Label lastName = new Label();
            lastName.ID = "lastNameLabel";
            lastName.Text = "<br />Last Name";
            lastName.CssClass = "clsCaption";

            _TableName.Rows[0].Cells[2].Controls.Add(lastName);

            //Suffix
            Label suffix = new Label();
            suffix.ID = "suffixLabel";
            suffix.Text = "<br />Suffix";
            suffix.CssClass = "clsCaptionOpt";

            _TableName.Rows[0].Cells[3].Controls.Add(suffix);

            _Table.Rows[1].Cells[1].Controls.Add(_TableName);

            //Phone START ---------------------------------------------------------------------------------------
            _Table.Rows[2].Cells[0].HorizontalAlign = HorizontalAlign.Right;
            _Table.Rows[2].Cells[0].Text = "Phone:";

            _Phone1 = new TextBox();
            _Phone1.ID = "phone1";
            _Phone1.MaxLength = 3;
            _Phone1.Width = Unit.Pixel(30);

            _Table.Rows[2].Cells[1].Controls.Add(_Phone1);

            Label dash1 = new Label();
            dash1.ID = "dash1";
            dash1.Text = "&nbsp;-&nbsp;";

            _Table.Rows[2].Cells[1].Controls.Add(dash1);

            _Phone2 = new TextBox();
            _Phone2.ID = "phone2";
            _Phone2.MaxLength = 3;
            _Phone2.Width = Unit.Pixel(30);

            _Table.Rows[2].Cells[1].Controls.Add(_Phone2);

            Label dash2 = new Label();
            dash2.ID = "dash2";
            dash2.Text = "&nbsp;-&nbsp;";

            _Table.Rows[2].Cells[1].Controls.Add(dash2);

            _Phone3 = new TextBox();
            _Phone3.ID = "phone3";
            _Phone3.MaxLength = 4;
            _Phone3.Width = Unit.Pixel(40);

            _Table.Rows[2].Cells[1].Controls.Add(_Phone3);

            Label extension = new Label();
            extension.ID = "extension";
            extension.Text = "&nbsp;Ext.&nbsp;";

            _Table.Rows[2].Cells[1].Controls.Add(extension);

            _Phone4 = new TextBox();
            _Phone4.ID = "phone4";
            _Phone4.MaxLength = 5;
            _Phone4.Width = Unit.Pixel(50);

            _Table.Rows[2].Cells[1].Controls.Add(_Phone4);
            //Phone END -----------------------------------------------------------------------------------------

            //Mailing Address START -----------------------------------------------------------------------------

            //Address Line 1
            _Table.Rows[3].Cells[0].HorizontalAlign = HorizontalAlign.Right;
            _Table.Rows[3].Cells[0].Text = "Mailing Address:";

            _AddressLine1 = new TextBox();
            _AddressLine1.ID = "addressLine1";
            _AddressLine1.Width = Unit.Pixel(380);

            _Table.Rows[3].Cells[1].Controls.Add(_AddressLine1);

            //Label Address Line 1
            Label addressLine1Label = new Label();
            addressLine1Label.ID = "addressLine1Label";
            addressLine1Label.Text = "<br />Street";
            addressLine1Label.CssClass = "clsCaption";

            _Table.Rows[3].Cells[1].Controls.Add(addressLine1Label);

            //Address Line 2
            _AddressLine2 = new TextBox();
            _AddressLine2.ID = "addressLine2";
            _AddressLine2.Width = Unit.Pixel(380);

            _Table.Rows[4].Cells[1].Controls.Add(_AddressLine2);

            //Label Address Line 2
            Label addressLine2Label = new Label();
            addressLine2Label.ID = "addressLine2Validator";
            addressLine2Label.Text = "<br />Street (cont.)";
            addressLine2Label.CssClass = "clsCaptionOpt";

            _Table.Rows[4].Cells[1].Controls.Add(addressLine2Label);

            _TableAddress = BuildTable(1, 3, "clsQuestion");
            _TableAddress.ID = "tableAddress";

            //City
            _City = new TextBox();
            _City.ID = "city";
            _City.Width = Unit.Pixel(240);

            _TableAddress.Rows[0].Cells[0].Controls.Add(_City);

            //Label City
            Label cityLabel = new Label();
            cityLabel.ID = "cityLabel";
            cityLabel.Text = "<br />City";
            cityLabel.CssClass = "clsCaption";

            _TableAddress.Rows[0].Cells[0].Controls.Add(cityLabel);

            //State
            _State = new DropDownList();
            _State.ID = "state";

            foreach (STATE pulledState in Enum.GetValues(typeof(STATE)))
            {
                if (pulledState == STATE.UNKNOWN)
                    _State.Items.Add(new ListItem(null, null));
                else if (pulledState.ToString().StartsWith("USA_"))
                    _State.Items.Add(new ListItem(pulledState.ToString().Replace("USA_", null), pulledState.ToString().Replace("USA_", null)));
            }

            _TableAddress.Rows[0].Cells[1].Controls.Add(_State);

            //Label State
            Label stateLabel = new Label();
            stateLabel.ID = "stateLabel";
            stateLabel.Text = "<br />State";
            stateLabel.CssClass = "clsCaption";

            _TableAddress.Rows[0].Cells[1].Controls.Add(stateLabel);

            //Zip
            _Zip = new TextBox();
            _Zip.ID = "zip";
            _Zip.MaxLength = 5;
            _Zip.Width = Unit.Pixel(50);

            _TableAddress.Rows[0].Cells[2].Controls.Add(_Zip);

            //Label Zip
            Label zipLabel = new Label();
            zipLabel.ID = "zipLabel";
            zipLabel.Text = "<br />Zip";
            zipLabel.CssClass = "clsCaption";

            _TableAddress.Rows[0].Cells[2].Controls.Add(zipLabel);

            _Table.Rows[5].Cells[1].Controls.Add(_TableAddress);

            //Mailing Address END -------------------------------------------------------------------------------

            //XML Transmittal START -----------------------------------------------------------------------------
            _Table.Rows[6].Cells[0].HorizontalAlign = HorizontalAlign.Right;
            _Table.Rows[6].Cells[0].Text = "Mailing Address:";

            _XMLTransmittal = new TextBox();
            _XMLTransmittal.ID = "xmlTransmittal";
            _XMLTransmittal.Width = Unit.Percentage(100);
            _XMLTransmittal.TextMode = TextBoxMode.MultiLine;
            _XMLTransmittal.Height = Unit.Pixel(400);

            _Table.Rows[6].Cells[1].Controls.Add(_XMLTransmittal);
            //XML Transmittal END -------------------------------------------------------------------------------


            //Submit START --------------------------------------------------------------------------------------

            Button submitButton = new Button();
            submitButton.ID = "submitButton";
            submitButton.Text = "Submit Transmittal";
            submitButton.Click += new EventHandler(submitButton_Click);

            _Table.Rows[8].Cells[1].HorizontalAlign = HorizontalAlign.Center;
            _Table.Rows[8].Cells[1].Controls.Add(submitButton);
            //Submit END ----------------------------------------------------------------------------------------

            this.gridPlaceHolder.Controls.Add(_Table);
        }

        void _XMLTransmittalButton_Click(object sender, EventArgs e)
        {
            Button xmlTransmittalButton = (Button)sender;

            if (xmlTransmittalButton.CommandName == "ViewXMLTransmittal")
            {
                Transmittal transmittal = BuildTransmittal();
                _XMLTransmittal.Text = SerializationHelper.SerializeToString(transmittal);

                //Hide User/Address textboxes.
                _Table.Rows[0].Attributes.Add("style", "display:none;");
                _Table.Rows[1].Attributes.Add("style", "display:none;");
                _Table.Rows[2].Attributes.Add("style", "display:none;");
                _Table.Rows[3].Attributes.Add("style", "display:none;");
                _Table.Rows[4].Attributes.Add("style", "display:none;");
                _Table.Rows[5].Attributes.Add("style", "display:none;");

                //Display XML textbox.
                _Table.Rows[6].Attributes.Add("style", "display:'';");

                //Modify command name and text of button.
                xmlTransmittalButton.CommandName = "ViewTextBoxes";
                xmlTransmittalButton.Text = "View Text Boxes";
            }
            else if (xmlTransmittalButton.CommandName == "ViewTextBoxes")
            {
                //Add modified info from _XMLTransmittal.Text to appropriate text boxes.
                //Transmittal transmittal = (Transmittal)SerializationHelper.DeserializeFromString(_XMLTransmittal.Text, typeof(Transmittal));

                //Hide TextBoxes.
                _Table.Rows[0].Attributes.Add("style", "display:'';");
                _Table.Rows[1].Attributes.Add("style", "display:'';");
                _Table.Rows[2].Attributes.Add("style", "display:'';");
                _Table.Rows[3].Attributes.Add("style", "display:'';");
                _Table.Rows[4].Attributes.Add("style", "display:'';");
                _Table.Rows[5].Attributes.Add("style", "display:'';");

                //Display XML TextBox.
                _Table.Rows[6].Attributes.Add("style", "display:none;");

                //Modify command name and text of button.
                xmlTransmittalButton.CommandName = "ViewXMLTransmittal";
                xmlTransmittalButton.Text = "View XML Transmittal";

                //Remove text from _XMLTransmittal.Text.
                _XMLTransmittal.Text = string.Empty;
            }
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (CheckForErrors())
                    BuildSamlRequest();
            }
        }

        private bool CheckForErrors()
        {
            string errorMessage = string.Empty;

            Label firstName = (Label)this.FindControl("firstNameLabel");
            Label lastName = (Label)this.FindControl("lastNameLabel");
            Label addressLine1 = (Label)this.FindControl("addressLine1Label");
            Label city = (Label)this.FindControl("cityLabel");
            Label state = (Label)this.FindControl("stateLabel");
            Label zip = (Label)this.FindControl("zipLabel");

            try
            {
                firstName.CssClass = "clsCaption";
                lastName.CssClass = "clsCaption";
                addressLine1.CssClass = "clsCaption";
                city.CssClass = "clsCaption";
                state.CssClass = "clsCaption";
                zip.CssClass = "clsCaption";

                _Table.Rows[0].Cells[0].ForeColor = Color.Black;
                _Table.Rows[2].Cells[0].ForeColor = Color.Black;
            }
            catch
            {
                Exception ArgumentException = new ArgumentException("A control was not found.");
                throw ArgumentException;
            }

            bool errorFound = false;

            Regex emailCheck = new System.Text.RegularExpressions.Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            Regex numericCheck = new System.Text.RegularExpressions.Regex(@"[0-9]");

            //Check for valid email.
            if (string.IsNullOrEmpty(this._EmailText.Text))
            {
                _Table.Rows[0].Cells[0].ForeColor = Color.Red;
                errorFound = true;
            }
            else if (!emailCheck.IsMatch(this._EmailText.Text))
            {
                _Table.Rows[0].Cells[0].ForeColor = Color.Red;
                errorMessage = "Please enter a valid e-mail address.";
                errorFound = true;
            }

            //Check for valid name.
            if (!errorFound && (!string.IsNullOrEmpty(this._FirstName.Text) || !string.IsNullOrEmpty(this._LastName.Text)))
            {
                if (string.IsNullOrEmpty(this._FirstName.Text))
                {
                    firstName.ForeColor = Color.Red;
                    errorFound = true;
                }
                else if (string.IsNullOrEmpty(this._LastName.Text))
                {
                    lastName.ForeColor = Color.Red;
                    errorFound = true;
                }
            }

            //Check for valid phone.
            if (!errorFound && (!string.IsNullOrEmpty(this._Phone1.Text) || !string.IsNullOrEmpty(this._Phone2.Text) || !string.IsNullOrEmpty(this._Phone3.Text) || !string.IsNullOrEmpty(this._Phone4.Text)))
            {
                if (string.IsNullOrEmpty(this._Phone1.Text) || string.IsNullOrEmpty(this._Phone2.Text) || string.IsNullOrEmpty(this._Phone3.Text))
                {
                    _Table.Rows[2].Cells[0].ForeColor = Color.Red;
                    errorFound = true;
                }
                else if (!numericCheck.IsMatch(this._Phone1.Text) || !numericCheck.IsMatch(this._Phone2.Text) || !numericCheck.IsMatch(this._Phone3.Text) || (!string.IsNullOrEmpty(this._Phone4.Text) && !numericCheck.IsMatch(this._Phone4.Text)))
                {
                    _Table.Rows[2].Cells[0].ForeColor = Color.Red;
                    errorMessage = "Please enter a valid numeric phone number.";
                    errorFound = true;
                }
            }

            //Check for valid address.
            if (!errorFound && (!string.IsNullOrEmpty(this._AddressLine1.Text) || !string.IsNullOrEmpty(this._City.Text) || !string.IsNullOrEmpty(this._State.Text) || !string.IsNullOrEmpty(this._Zip.Text)))
            {
                if (string.IsNullOrEmpty(this._AddressLine1.Text))
                {
                    addressLine1.ForeColor = Color.Red;
                    errorFound = true;
                }
                else if (string.IsNullOrEmpty(this._City.Text))
                {
                    city.ForeColor = Color.Red;
                    errorFound = true;
                }
                else if (string.IsNullOrEmpty(this._State.Text))
                {
                    state.ForeColor = Color.Red;
                    errorFound = true;
                }
                else if (string.IsNullOrEmpty(this._Zip.Text))
                {
                    zip.ForeColor = Color.Red;
                    errorFound = true;
                }
                else if (!numericCheck.IsMatch(this._Zip.Text))
                {
                    zip.ForeColor = Color.Red;
                    errorMessage = "Please enter a valid numeric zip code.";
                    errorFound = true;
                }
            }

            if (errorFound)
            {
                if (string.IsNullOrEmpty(errorMessage))
                    errorMessage = "Required information is missing. See field(s) highlighted in red.";

                ClientScript.RegisterStartupScript(typeof(Page), "ErrorsFound",
                    @"
                    <script language=""javascript"">
                    <!--
	                    alert(""" + errorMessage + @""");
                    //-->
		            </script>");
            }

            return !errorFound;
        }

        private void BuildSamlRequest()
        {
            ClientScript.RegisterStartupScript(typeof(Page), "OpaqueDivider",
                @"
                <script language=""javascript"">
                <!--
                    var dividerID = '" + this.SamlAgentDiv.ClientID + @"';
                    var divider = document.getElementById(dividerID);

                    divider.style.visibility = 'visible';
                //-->
	            </script>");

            //Creating SAML response
            X509Certificate2 vendorCertificate = GetVendorCertificate();
            X509Certificate2 selerixCertificate = GetSelerixCertificate();

            //string assertionConsumerServiceURL = "SamlResponse.aspx";
            string assertionConsumerServiceURL = "http://localhost:49000/login.aspx?Path=SAML_TEST";

            string audienceName = "whatever audience";

            SAMLResponse samlResponse = new SAMLResponse();
            samlResponse.Destination = assertionConsumerServiceURL;

            Issuer issuer = new Issuer("Vendor");
            samlResponse.Issuer = issuer;
            samlResponse.Status = new Status(SAMLIdentifiers.PrimaryStatusCodes.Success, null);

            SAMLAssertion samlAssertion = new SAMLAssertion();
            samlAssertion.Issuer = issuer;

            Subject subject = null;

            //subject = new Subject(new EncryptedID(new NameID(this._EmailText.Text), selerixCertificate, new EncryptionMethod(EncryptedXml.XmlEncTripleDESUrl)));
            subject = new Subject(new NameID(this._EmailText.Text));

            SubjectConfirmation subjectConfirmation = new SubjectConfirmation(SAMLIdentifiers.SubjectConfirmationMethods.Bearer);
            SubjectConfirmationData subjectConfirmationData = new SubjectConfirmationData();

            subjectConfirmationData.Recipient = assertionConsumerServiceURL;
            subjectConfirmationData.NotOnOrAfter = DateTime.UtcNow.AddHours(1);
            subjectConfirmation.SubjectConfirmationData = subjectConfirmationData;

            subject.SubjectConfirmations.Add(subjectConfirmation);
            samlAssertion.Subject = subject;

            Conditions conditions = new Conditions(new TimeSpan(1, 0, 0));
            AudienceRestriction audienceRestriction = new AudienceRestriction();

            audienceRestriction.Audiences.Add(new Audience(audienceName));
            conditions.ConditionsList.Add(audienceRestriction);
            samlAssertion.Conditions = conditions;

            AuthnStatement authnStatement = new AuthnStatement();
            authnStatement.AuthnContext = new AuthnContext();
            authnStatement.AuthnContext.AuthnContextClassRef = new AuthnContextClassRef(SAMLIdentifiers.AuthnContextClasses.Unspecified);

            samlAssertion.Statements.Add(authnStatement);

            AttributeStatement attributeStatement = new AttributeStatement();

            Transmittal transmittal = BuildTransmittal();

            if (transmittal != null && !string.IsNullOrEmpty(this._FirstName.Text) && !string.IsNullOrEmpty(this._LastName.Text))
            {
                attributeStatement.Attributes.Add(new SAMLAttribute("Transmittal", SAMLIdentifiers.AttributeNameFormats.Basic, null, SerializationHelper.SerializeToString(transmittal)));
            }

            samlAssertion.Statements.Add(attributeStatement);

//          EncryptedAssertion encryptedAssertion = new EncryptedAssertion(samlAssertion, selerixCertificate, new EncryptionMethod(EncryptedXml.XmlEncTripleDESUrl));
//          samlResponse.Assertions.Add(encryptedAssertion);
            samlResponse.Assertions.Add(samlAssertion);

            //Created SAML response

            //Sending SAML response

            // Serialize the SAML response for transmission.
            XmlElement samlResponseXml = samlResponse.ToXml();

            // Sign the SAML response.
            SAMLMessageSignature.Generate(samlResponseXml, vendorCertificate.PrivateKey, vendorCertificate);

            HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache");
            HttpContext.Current.Response.AddHeader("Pragma", "no-cache");

            IdentityProvider.SendSAMLResponseByHTTPPost(HttpContext.Current.Response, assertionConsumerServiceURL, samlResponseXml, "");// for test purposes
        }

        private Transmittal BuildTransmittal()
        {
            Transmittal transmittal = new Transmittal();
            transmittal.SenderID = Guid.NewGuid();

            string agentName = !string.IsNullOrEmpty(this._MiddleInitial.Text) ? this._FirstName.Text + " " + this._MiddleInitial.Text + " " : this._FirstName.Text + " ";
            agentName += !string.IsNullOrEmpty(this._Suffix.Text) ? this._LastName.Text + " " + this._Suffix.Text : this._LastName.Text;

            Agent agent = new Agent();

            agent.ID = Guid.NewGuid().ToString();
            agent.Email = this._EmailText.Text;

            agent.FirstName = this._FirstName.Text;
            agent.MiddleInitial = this._MiddleInitial.Text;
            agent.LastName = this._LastName.Text;
            agent.Name = agentName;
            agent.PhoneWork = this._Phone1.Text + this._Phone2.Text + this._Phone3.Text + this._Phone4.Text;

            agent.Address = new Address();
            agent.Address.Line1 = this._AddressLine1.Text;
            agent.Address.Line2 = this._AddressLine2.Text != null ? this._AddressLine2.Text : null;
            agent.Address.City = this._City.Text;
            agent.Address.State = this._State.SelectedValue;
            agent.Address.Zip = this._Zip.Text;

            transmittal.Agents = new AgentCollection();
            transmittal.Agents.Add(agent);

            return transmittal;
        }

        /// <summary>
        /// Encryption certificate.
        /// </summary>
        /// <returns></returns>
        private X509Certificate2 GetSelerixCertificate()
        {
            X509Certificate2 result = Utils.GetSelerixCertificateFromStorage();

            if (result == null)
                result = Utils.GetCertificateFromFileSystem(Path.Combine(Request.PhysicalApplicationPath, "Server.pfx"), "123");

            return result;
        }

        /// <summary>
        /// Identification certificate.
        /// </summary>
        /// <returns></returns>
        private X509Certificate2 GetVendorCertificate()
        {
            X509Certificate2 result = Utils.GetVendorCertificateFromStorage();

            if (result == null)
                result = Utils.GetCertificateFromFileSystem(Path.Combine(Request.PhysicalApplicationPath, "Client.pfx"), "123");

            return result;
        }

        private static System.Web.UI.WebControls.Table BuildTable(int intRows, int intCols, string tableCellClass)
        {
            System.Web.UI.WebControls.Table table = new System.Web.UI.WebControls.Table();

            for (int r = 0; r < intRows; r++)
                table.Rows.Add(CreateTableRow(intCols, tableCellClass));

            return table;
        }

        private static TableRow CreateTableRow(int intCols, string tableCellClass)
        {
            TableRow tr = new TableRow();

            for (int c = 0; c < intCols; c++)
            {
                TableCell tc = new TableCell();

                if (!string.IsNullOrEmpty(tableCellClass))
                    tc.CssClass = tableCellClass;

                tr.Cells.Add(tc);
            }

            return tr;
        }
    }
}