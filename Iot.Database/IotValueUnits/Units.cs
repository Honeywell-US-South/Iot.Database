namespace Iot.Database.IotValueUnits;


public partial struct Units
{
    public static IotUnit no_unit => new IotUnit("None", "no_unit", "");

    public struct All
    {
        // Acceleration
        public static IotUnit meters_per_second_per_second => new IotUnit("Acceleration", "meters_per_second_per_second", "m/s²");
        public static IotUnit standard_gravity => new IotUnit("Acceleration", "standard_gravity", "g");

        // Angular
        public static IotUnit degrees_angular => new IotUnit("Angular", "degrees_angular", "°");
        public static IotUnit radians => new IotUnit("Angular", "radians", "rad");
        public static IotUnit radians_per_second => new IotUnit("Angular", "radians_per_second", "rad/s");
        public static IotUnit revolutions_per_minute => new IotUnit("Angular", "revolutions_per_minute", "RPM");

        // Area
        public static IotUnit square_centimeters => new IotUnit("Area", "square_centimeters", "cm²");
        public static IotUnit square_feet => new IotUnit("Area", "square_feet", "ft²");
        public static IotUnit square_inches => new IotUnit("Area", "square_inches", "in²");
        public static IotUnit square_meters => new IotUnit("Area", "square_meters", "m²");

        // Capacitance
        public static IotUnit farads => new IotUnit("Capacitance", "farads", "F");
        public static IotUnit microfarads => new IotUnit("Capacitance", "microfarads", "μF");
        public static IotUnit nanofarads => new IotUnit("Capacitance", "nanofarads", "nF");
        public static IotUnit picofarads => new IotUnit("Capacitance", "picofarads", "pF");

        // Concentration
        public static IotUnit mole_percent => new IotUnit("Concentration", "mole_percent", "mol%");
        public static IotUnit parts_per_billion => new IotUnit("Concentration", "parts_per_billion", "ppb");
        public static IotUnit parts_per_million => new IotUnit("Concentration", "parts_per_million", "ppm");
        public static IotUnit percent => new IotUnit("Concentration", "percent", "%");
        public static IotUnit percent_obscuration_per_foot => new IotUnit("Concentration", "percent_obscuration_per_foot", "%/ft");
        public static IotUnit percent_obscuration_per_meter => new IotUnit("Concentration", "percent_obscuration_per_meter", "%/m");
        public static IotUnit percent_per_second => new IotUnit("Concentration", "percent_per_second", "%/s");
        public static IotUnit per_mille => new IotUnit("Concentration", "per_mille", "‰");

        // Currency
        public static IotUnit afghan_afghani => new IotUnit("Currency", "afghan_afghani", "؋");
        public static IotUnit albanian_lek => new IotUnit("Currency", "albanian_lek", "L");
        public static IotUnit algerian_dinar => new IotUnit("Currency", "algerian_dinar", "د.ج");
        public static IotUnit angolan_kwanza => new IotUnit("Currency", "angolan_kwanza", "Kz");
        public static IotUnit argentine_peso => new IotUnit("Currency", "argentine_peso", "$");
        public static IotUnit armenian_dram => new IotUnit("Currency", "armenian_dram", "֏");
        public static IotUnit aruban_florin => new IotUnit("Currency", "aruban_florin", "ƒ");
        public static IotUnit australian_dollar => new IotUnit("Currency", "australian_dollar", "$");
        public static IotUnit azerbaijani_manat => new IotUnit("Currency", "azerbaijani_manat", "₼");
        public static IotUnit bahamian_dollar => new IotUnit("Currency", "bahamian_dollar", "$");
        public static IotUnit bahraini_dinar => new IotUnit("Currency", "bahraini_dinar", ".د.ب");
        public static IotUnit bangladeshi_taka => new IotUnit("Currency", "bangladeshi_taka", "৳");
        public static IotUnit barbadian_dollar => new IotUnit("Currency", "barbadian_dollar", "$");
        public static IotUnit belarusian_ruble => new IotUnit("Currency", "belarusian_ruble", "Br");
        public static IotUnit belize_dollar => new IotUnit("Currency", "belize_dollar", "$");
        public static IotUnit bermudian_dollar => new IotUnit("Currency", "bermudian_dollar", "$");
        public static IotUnit bhutanese_ngultrum => new IotUnit("Currency", "bhutanese_ngultrum", "Nu.");
        public static IotUnit bolivian_boliviano => new IotUnit("Currency", "bolivian_boliviano", "Bs.");
        public static IotUnit bosnia_and_herzegovina_convertible_mark => new IotUnit("Currency", "bosnia_and_herzegovina_convertible_mark", "KM");
        public static IotUnit botswana_pula => new IotUnit("Currency", "botswana_pula", "P");
        public static IotUnit brazilian_real => new IotUnit("Currency", "brazilian_real", "R$");
        public static IotUnit brunei_dollar => new IotUnit("Currency", "brunei_dollar", "$");
        public static IotUnit bulgarian_lev => new IotUnit("Currency", "bulgarian_lev", "лв");
        public static IotUnit burundian_franc => new IotUnit("Currency", "burundian_franc", "FBu");
        public static IotUnit cape_verdean_escudo => new IotUnit("Currency", "cape_verdean_escudo", "$");
        public static IotUnit cambodian_riel => new IotUnit("Currency", "cambodian_riel", "៛");
        public static IotUnit canadian_dollar => new IotUnit("Currency", "canadian_dollar", "$");
        public static IotUnit cayman_islands_dollar => new IotUnit("Currency", "cayman_islands_dollar", "$");
        public static IotUnit central_african_cfa_franc => new IotUnit("Currency", "central_african_cfa_franc", "Fr");
        public static IotUnit chilean_peso => new IotUnit("Currency", "chilean_peso", "$");
        public static IotUnit chinese_yuan => new IotUnit("Currency", "chinese_yuan", "¥");
        public static IotUnit colombian_peso => new IotUnit("Currency", "colombian_peso", "$");
        public static IotUnit comorian_franc => new IotUnit("Currency", "comorian_franc", "CF");
        public static IotUnit congolese_franc => new IotUnit("Currency", "congolese_franc", "FC");
        public static IotUnit costa_rican_colon => new IotUnit("Currency", "costa_rican_colon", "₡");
        public static IotUnit croatian_kuna => new IotUnit("Currency", "croatian_kuna", "kn");
        public static IotUnit cuban_convertible_peso => new IotUnit("Currency", "cuban_convertible_peso", "$");
        public static IotUnit cuban_peso => new IotUnit("Currency", "cuban_peso", "$");
        public static IotUnit czech_koruna => new IotUnit("Currency", "czech_koruna", "Kč");
        public static IotUnit danish_krone => new IotUnit("Currency", "danish_krone", "kr");
        public static IotUnit djiboutian_franc => new IotUnit("Currency", "djiboutian_franc", "Fdj");
        public static IotUnit dominican_peso => new IotUnit("Currency", "dominican_peso", "$");
        public static IotUnit east_caribbean_dollar => new IotUnit("Currency", "east_caribbean_dollar", "$");
        public static IotUnit egyptian_pound => new IotUnit("Currency", "egyptian_pound", "£");
        public static IotUnit eritrean_nakfa => new IotUnit("Currency", "eritrean_nakfa", "Nfk");
        public static IotUnit ethiopian_birr => new IotUnit("Currency", "ethiopian_birr", "Br");
        public static IotUnit euro => new IotUnit("Currency", "euro", "€");
        public static IotUnit falkland_islands_pound => new IotUnit("Currency", "falkland_islands_pound", "£");
        public static IotUnit fiji_dollar => new IotUnit("Currency", "fiji_dollar", "$");
        public static IotUnit gambian_dalasi => new IotUnit("Currency", "gambian_dalasi", "D");
        public static IotUnit georgian_lari => new IotUnit("Currency", "georgian_lari", "₾");
        public static IotUnit ghanaian_cedi => new IotUnit("Currency", "ghanaian_cedi", "₵");
        public static IotUnit gibraltar_pound => new IotUnit("Currency", "gibraltar_pound", "£");
        public static IotUnit guatemalan_quetzal => new IotUnit("Currency", "guatemalan_quetzal", "Q");
        public static IotUnit guinean_franc => new IotUnit("Currency", "guinean_franc", "FG");
        public static IotUnit guyanese_dollar => new IotUnit("Currency", "guyanese_dollar", "$");
        public static IotUnit haitian_gourde => new IotUnit("Currency", "haitian_gourde", "G");
        public static IotUnit honduran_lempira => new IotUnit("Currency", "honduran_lempira", "L");
        public static IotUnit hong_kong_dollar => new IotUnit("Currency", "hong_kong_dollar", "$");
        public static IotUnit hungarian_forint => new IotUnit("Currency", "hungarian_forint", "Ft");
        public static IotUnit icelandic_krona => new IotUnit("Currency", "icelandic_krona", "kr");
        public static IotUnit indian_rupee => new IotUnit("Currency", "indian_rupee", "₹");
        public static IotUnit indonesian_rupiah => new IotUnit("Currency", "indonesian_rupiah", "Rp");
        public static IotUnit iranian_rial => new IotUnit("Currency", "iranian_rial", "﷼");
        public static IotUnit iraqi_dinar => new IotUnit("Currency", "iraqi_dinar", "ع.د");
        public static IotUnit israeli_new_shekel => new IotUnit("Currency", "israeli_new_shekel", "₪");
        public static IotUnit jamaican_dollar => new IotUnit("Currency", "jamaican_dollar", "$");
        public static IotUnit japanese_yen => new IotUnit("Currency", "japanese_yen", "¥");
        public static IotUnit jordanian_dinar => new IotUnit("Currency", "jordanian_dinar", "د.ا");
        public static IotUnit kazakhstani_tenge => new IotUnit("Currency", "kazakhstani_tenge", "₸");
        public static IotUnit kenyan_shilling => new IotUnit("Currency", "kenyan_shilling", "KSh");
        public static IotUnit kuwaiti_dinar => new IotUnit("Currency", "kuwaiti_dinar", "د.ك");
        public static IotUnit kyrgyzstani_som => new IotUnit("Currency", "kyrgyzstani_som", "лв");
        public static IotUnit lao_kip => new IotUnit("Currency", "lao_kip", "₭");
        public static IotUnit lebanese_pound => new IotUnit("Currency", "lebanese_pound", "ل.ل");
        public static IotUnit lesotho_loti => new IotUnit("Currency", "lesotho_loti", "L");
        public static IotUnit liberian_dollar => new IotUnit("Currency", "liberian_dollar", "$");
        public static IotUnit libyan_dinar => new IotUnit("Currency", "libyan_dinar", "ل.د");
        public static IotUnit macanese_pataca => new IotUnit("Currency", "macanese_pataca", "MOP$");
        public static IotUnit malagasy_ariary => new IotUnit("Currency", "malagasy_ariary", "Ar");
        public static IotUnit malawian_kwacha => new IotUnit("Currency", "malawian_kwacha", "MK");
        public static IotUnit malaysian_ringgit => new IotUnit("Currency", "malaysian_ringgit", "RM");
        public static IotUnit maldivian_rufiyaa => new IotUnit("Currency", "maldivian_rufiyaa", "ރ.");
        public static IotUnit mauritanian_ouguiya => new IotUnit("Currency", "mauritanian_ouguiya", "UM");
        public static IotUnit mauritian_rupee => new IotUnit("Currency", "mauritian_rupee", "₨");
        public static IotUnit mexican_peso => new IotUnit("Currency", "mexican_peso", "$");
        public static IotUnit moldovan_leu => new IotUnit("Currency", "moldovan_leu", "L");
        public static IotUnit mongolian_togrog => new IotUnit("Currency", "mongolian_togrog", "₮");
        public static IotUnit moroccan_dirham => new IotUnit("Currency", "moroccan_dirham", "د.م.");
        public static IotUnit mozambican_metical => new IotUnit("Currency", "mozambican_metical", "MT");
        public static IotUnit myanmar_kyat => new IotUnit("Currency", "myanmar_kyat", "K");
        public static IotUnit namibian_dollar => new IotUnit("Currency", "namibian_dollar", "$");
        public static IotUnit nepalese_rupee => new IotUnit("Currency", "nepalese_rupee", "₨");
        public static IotUnit netherlands_antillean_guilder => new IotUnit("Currency", "netherlands_antillean_guilder", "ƒ");
        public static IotUnit new_taiwan_dollar => new IotUnit("Currency", "new_taiwan_dollar", "$");
        public static IotUnit new_zealand_dollar => new IotUnit("Currency", "new_zealand_dollar", "$");
        public static IotUnit nicaraguan_cordoba => new IotUnit("Currency", "nicaraguan_cordoba", "C$");
        public static IotUnit nigerian_naira => new IotUnit("Currency", "nigerian_naira", "₦");
        public static IotUnit north_korean_won => new IotUnit("Currency", "north_korean_won", "₩");
        public static IotUnit norwegian_krone => new IotUnit("Currency", "norwegian_krone", "kr");
        public static IotUnit omani_rial => new IotUnit("Currency", "omani_rial", "ر.ع.");
        public static IotUnit pakistani_rupee => new IotUnit("Currency", "pakistani_rupee", "₨");
        public static IotUnit panamanian_balboa => new IotUnit("Currency", "panamanian_balboa", "B/.");
        public static IotUnit papua_new_guinean_kina => new IotUnit("Currency", "papua_new_guinean_kina", "K");
        public static IotUnit paraguayan_guarani => new IotUnit("Currency", "paraguayan_guarani", "₲");
        public static IotUnit peruvian_sol => new IotUnit("Currency", "peruvian_sol", "S/");
        public static IotUnit philippine_peso => new IotUnit("Currency", "philippine_peso", "₱");
        public static IotUnit polish_zloty => new IotUnit("Currency", "polish_zloty", "zł");
        public static IotUnit qatar_riyal => new IotUnit("Currency", "qatar_riyal", "ر.ق");
        public static IotUnit romanian_leu => new IotUnit("Currency", "romanian_leu", "lei");
        public static IotUnit russian_ruble => new IotUnit("Currency", "russian_ruble", "₽");
        public static IotUnit rwandan_franc => new IotUnit("Currency", "rwandan_franc", "FRw");
        public static IotUnit saint_helena_pound => new IotUnit("Currency", "saint_helena_pound", "£");
        public static IotUnit samoan_tala => new IotUnit("Currency", "samoan_tala", "WS$");
        public static IotUnit saudi_riyal => new IotUnit("Currency", "saudi_riyal", "ر.س");
        public static IotUnit serbian_dinar => new IotUnit("Currency", "serbian_dinar", "дин.");
        public static IotUnit seychellois_rupee => new IotUnit("Currency", "seychellois_rupee", "₨");
        public static IotUnit sierra_leonean_leone => new IotUnit("Currency", "sierra_leonean_leone", "Le");
        public static IotUnit singapore_dollar => new IotUnit("Currency", "singapore_dollar", "$");
        public static IotUnit solomon_islands_dollar => new IotUnit("Currency", "solomon_islands_dollar", "$");
        public static IotUnit somali_shilling => new IotUnit("Currency", "somali_shilling", "Sh");
        public static IotUnit south_african_rand => new IotUnit("Currency", "south_african_rand", "R");
        public static IotUnit south_korean_won => new IotUnit("Currency", "south_korean_won", "₩");
        public static IotUnit south_sudanese_pound => new IotUnit("Currency", "south_sudanese_pound", "£");
        public static IotUnit sri_lankan_rupee => new IotUnit("Currency", "sri_lankan_rupee", "₨");
        public static IotUnit sudanese_pound => new IotUnit("Currency", "sudanese_pound", "ج.س.");
        public static IotUnit surinamese_dollar => new IotUnit("Currency", "surinamese_dollar", "$");
        public static IotUnit swazi_lilangeni => new IotUnit("Currency", "swazi_lilangeni", "L");
        public static IotUnit swedish_krona => new IotUnit("Currency", "swedish_krona", "kr");
        public static IotUnit swiss_franc => new IotUnit("Currency", "swiss_franc", "Fr");
        public static IotUnit syrian_pound => new IotUnit("Currency", "syrian_pound", "£");
        public static IotUnit taiwanese_dollar => new IotUnit("Currency", "taiwanese_dollar", "NT$");
        public static IotUnit tajikistani_somoni => new IotUnit("Currency", "tajikistani_somoni", "ЅМ");
        public static IotUnit tanzanian_shilling => new IotUnit("Currency", "tanzanian_shilling", "Sh");
        public static IotUnit thai_baht => new IotUnit("Currency", "thai_baht", "฿");
        public static IotUnit tonga_paanga => new IotUnit("Currency", "tonga_paanga", "T$");
        public static IotUnit trinidad_and_tobago_dollar => new IotUnit("Currency", "trinidad_and_tobago_dollar", "$");
        public static IotUnit tunisian_dinar => new IotUnit("Currency", "tunisian_dinar", "د.ت");
        public static IotUnit turkish_lira => new IotUnit("Currency", "turkish_lira", "₺");
        public static IotUnit turkmenistani_manat => new IotUnit("Currency", "turkmenistani_manat", "m");
        public static IotUnit ugandan_shilling => new IotUnit("Currency", "ugandan_shilling", "USh");
        public static IotUnit ukrainian_hryvnia => new IotUnit("Currency", "ukrainian_hryvnia", "₴");
        public static IotUnit united_arab_emirates_dirham => new IotUnit("Currency", "united_arab_emirates_dirham", "د.إ");
        public static IotUnit uruguayan_peso => new IotUnit("Currency", "uruguayan_peso", "$U");
        public static IotUnit uzbekistani_som => new IotUnit("Currency", "uzbekistani_som", "so'm");
        public static IotUnit vanuatu_vatu => new IotUnit("Currency", "vanuatu_vatu", "VT");
        public static IotUnit venezuelan_bolivar => new IotUnit("Currency", "venezuelan_bolivar", "Bs.");
        public static IotUnit vietnamese_dong => new IotUnit("Currency", "vietnamese_dong", "₫");
        public static IotUnit yemeni_rial => new IotUnit("Currency", "yemeni_rial", "﷼");
        public static IotUnit zambian_kwacha => new IotUnit("Currency", "zambian_kwacha", "ZK");
        public static IotUnit zimbabwean_dollar => new IotUnit("Currency", "zimbabwean_dollar", "$");

        // DataRate
        public static IotUnit bits_per_second => new IotUnit("DataRate", "bits_per_second", "bps");
        public static IotUnit gigabits_per_second => new IotUnit("DataRate", "gigabits_per_second", "Gbps");
        public static IotUnit kilobits_per_second => new IotUnit("DataRate", "kilobits_per_second", "kbps");
        public static IotUnit megabits_per_second => new IotUnit("DataRate", "megabits_per_second", "Mbps");

        // DataStorage
        public static IotUnit bytes => new IotUnit("DataStorage", "bytes", "B");
        public static IotUnit exabytes => new IotUnit("DataStorage", "exabytes", "EB");
        public static IotUnit gigabytes => new IotUnit("DataStorage", "gigabytes", "GB");
        public static IotUnit kilobytes => new IotUnit("DataStorage", "kilobytes", "KB");
        public static IotUnit megabytes => new IotUnit("DataStorage", "megabytes", "MB");
        public static IotUnit petabytes => new IotUnit("DataStorage", "petabytes", "PB");
        public static IotUnit terabytes => new IotUnit("DataStorage", "terabytes", "TB");
        public static IotUnit yottabytes => new IotUnit("DataStorage", "yottabytes", "YB");
        public static IotUnit zettabytes => new IotUnit("DataStorage", "zettabytes", "ZB");

        // ElectricCharge
        public static IotUnit ampere_hours => new IotUnit("ElectricCharge", "ampere_hours", "Ah");
        public static IotUnit coulombs => new IotUnit("ElectricCharge", "coulombs", "C");

        // ElectricPotential
        public static IotUnit kilovolts => new IotUnit("ElectricPotential", "kilovolts", "kV");
        public static IotUnit millivolts => new IotUnit("ElectricPotential", "millivolts", "mV");
        public static IotUnit volts => new IotUnit("ElectricPotential", "volts", "V");

        // ElectricResistance
        public static IotUnit kilohms => new IotUnit("ElectricResistance", "kilohms", "kΩ");
        public static IotUnit megohms => new IotUnit("ElectricResistance", "megohms", "MΩ");
        public static IotUnit milliohms => new IotUnit("ElectricResistance", "milliohms", "mΩ");
        public static IotUnit ohms => new IotUnit("ElectricResistance", "ohms", "Ω");

        // Electrical
        public static IotUnit amperes => new IotUnit("Electrical", "amperes", "A");
        public static IotUnit amperes_per_meter => new IotUnit("Electrical", "amperes_per_meter", "A/m");
        public static IotUnit amperes_per_square_meter => new IotUnit("Electrical", "amperes_per_square_meter", "A/m²");
        public static IotUnit ampere_square_meters => new IotUnit("Electrical", "ampere_square_meters", "A·m²");
        public static IotUnit bars => new IotUnit("Electrical", "bars", "bar");
        public static IotUnit decibels => new IotUnit("Electrical", "decibels", "dB");
        public static IotUnit decibels_millivolt => new IotUnit("Electrical", "decibels_millivolt", "dBmV");
        public static IotUnit decibels_volt => new IotUnit("Electrical", "decibels_volt", "dBV");
        public static IotUnit degrees_phase => new IotUnit("Electrical", "degrees_phase", "°");
        public static IotUnit henrys => new IotUnit("Electrical", "henrys", "H");
        public static IotUnit kilovolt_amperes => new IotUnit("Electrical", "kilovolt_amperes", "kVA");
        public static IotUnit kilovolt_amperes_reactive => new IotUnit("Electrical", "kilovolt_amperes_reactive", "kVAR");
        public static IotUnit megavolt_amperes => new IotUnit("Electrical", "megavolt_amperes", "MVA");
        public static IotUnit megavolt_amperes_reactive => new IotUnit("Electrical", "megavolt_amperes_reactive", "MVAR");
        public static IotUnit microamperes => new IotUnit("Electrical", "microamperes", "μA");
        public static IotUnit microhenrys => new IotUnit("Electrical", "microhenrys", "μH");
        public static IotUnit milliamperes => new IotUnit("Electrical", "milliamperes", "mA");
        public static IotUnit millihenrys => new IotUnit("Electrical", "millihenrys", "mH");
        public static IotUnit millivolt_amperes => new IotUnit("Electrical", "millivolt_amperes", "mVA");
        public static IotUnit nanohenrys => new IotUnit("Electrical", "nanohenrys", "nH");
        public static IotUnit siemens => new IotUnit("Electrical", "siemens", "S");
        public static IotUnit teslas => new IotUnit("Electrical", "teslas", "T");
        public static IotUnit volts_dc => new IotUnit("Electrical", "volts_dc", "VDC");
        public static IotUnit watts => new IotUnit("Electrical", "watts", "W");
        public static IotUnit watts_per_meter_kelvin => new IotUnit("Electrical", "watts_per_meter_kelvin", "W/mK");
        public static IotUnit watt_seconds => new IotUnit("Electrical", "watt_seconds", "Ws");
        public static IotUnit watt_hours => new IotUnit("Electrical", "watt_hours", "Wh");
        public static IotUnit watt_per_square_meter => new IotUnit("Electrical", "watt_per_square_meter", "W/m²");

        // Energy Units
        public static IotUnit ampere_seconds => new IotUnit("Energy", "ampere_seconds", "As");
        public static IotUnit btus => new IotUnit("Energy", "btus", "BTU");
        public static IotUnit joules => new IotUnit("Energy", "joules", "J");
        public static IotUnit kilo_btus => new IotUnit("Energy", "kilo_btus", "kBTU");
        public static IotUnit kilojoules => new IotUnit("Energy", "kilojoules", "kJ");
        public static IotUnit kilojoules_per_kilogram => new IotUnit("Energy", "kilojoules_per_kilogram", "kJ/kg");
        public static IotUnit kilovolt_ampere_hours => new IotUnit("Energy", "kilovolt_ampere_hours", "kVAh");
        public static IotUnit kilovolt_ampere_hours_reactive => new IotUnit("Energy", "kilovolt_ampere_hours_reactive", "kVARh");
        public static IotUnit kilowatt_hours => new IotUnit("Energy", "kilowatt_hours", "kWh");
        public static IotUnit kilowatt_hours_reactive => new IotUnit("Energy", "kilowatt_hours_reactive", "kWh");
        public static IotUnit megajoules => new IotUnit("Energy", "megajoules", "MJ");
        public static IotUnit megavolt_ampere_hours => new IotUnit("Energy", "megavolt_ampere_hours", "MVAh");
        public static IotUnit megavolt_ampere_hours_reactive => new IotUnit("Energy", "megavolt_ampere_hours_reactive", "MVARh");
        public static IotUnit megawatt_hours => new IotUnit("Energy", "megawatt_hours", "MWh");
        public static IotUnit megawatt_hours_reactive => new IotUnit("Energy", "megawatt_hours_reactive", "MWh");
        public static IotUnit ton_hours => new IotUnit("Energy", "ton_hours", "ton-h");
        public static IotUnit volt_ampere_hours => new IotUnit("Energy", "volt_ampere_hours", "VAh");
        public static IotUnit volt_ampere_hours_reactive => new IotUnit("Energy", "volt_ampere_hours_reactive", "VARh");
        public static IotUnit volt_square_hours => new IotUnit("Energy", "volt_square_hours", "V²h");
        public static IotUnit watt_hours_energy => new IotUnit("Energy", "watt_hours", "Wh");
        public static IotUnit watt_hours_reactive => new IotUnit("Energy", "watt_hours_reactive", "Wh");

        // Energy Density Units
        public static IotUnit joules_per_cubic_meter => new IotUnit("EnergyDensity", "joules_per_cubic_meter", "J/m³");
        public static IotUnit kilowatt_hours_per_square_foot => new IotUnit("EnergyDensity", "kilowatt_hours_per_square_foot", "kWh/ft²");
        public static IotUnit kilowatt_hours_per_square_meter => new IotUnit("EnergyDensity", "kilowatt_hours_per_square_meter", "kWh/m²");
        public static IotUnit megajoules_per_square_foot => new IotUnit("EnergyDensity", "megajoules_per_square_foot", "MJ/ft²");
        public static IotUnit megajoules_per_square_meter => new IotUnit("EnergyDensity", "megajoules_per_square_meter", "MJ/m²");
        public static IotUnit watt_hours_per_cubic_meter => new IotUnit("EnergyDensity", "watt_hours_per_cubic_meter", "Wh/m³");

        // Energy Specific Units
        public static IotUnit joule_seconds => new IotUnit("EnergySpecific", "joule_seconds", "Js");

        // Enthalpy Units
        public static IotUnit btus_per_pound => new IotUnit("Enthalpy", "btus_per_pound", "BTU/lb");
        public static IotUnit btus_per_pound_dry_air => new IotUnit("Enthalpy", "btus_per_pound_dry_air", "BTU/lb");
        public static IotUnit joules_per_degree_kelvin => new IotUnit("Enthalpy", "joules_per_degree_kelvin", "J/K");
        public static IotUnit joules_per_kilogram_dry_air => new IotUnit("Enthalpy", "joules_per_kilogram_dry_air", "J/kg");
        public static IotUnit joules_per_kilogram_degree_kelvin => new IotUnit("Enthalpy", "joules_per_kilogram_degree_kelvin", "J/(kg·K)");
        public static IotUnit kilojoules_per_degree_kelvin => new IotUnit("Enthalpy", "kilojoules_per_degree_kelvin", "kJ/K");
        public static IotUnit kilojoules_per_kilogram_dry_air => new IotUnit("Enthalpy", "kilojoules_per_kilogram_dry_air", "kJ/kg");
        public static IotUnit megajoules_per_degree_kelvin => new IotUnit("Enthalpy", "megajoules_per_degree_kelvin", "MJ/K");
        public static IotUnit megajoules_per_kilogram_dry_air => new IotUnit("Enthalpy", "megajoules_per_kilogram_dry_air", "MJ/kg");


        // Force Units
        public static IotUnit newton => new IotUnit("Force", "newton", "N");

        // Frequency Units
        public static IotUnit cycles_per_hour => new IotUnit("Frequency", "cycles_per_hour", "cph");
        public static IotUnit cycles_per_minute => new IotUnit("Frequency", "cycles_per_minute", "cpm");
        public static IotUnit hertz => new IotUnit("Frequency", "hertz", "Hz");
        public static IotUnit kilohertz => new IotUnit("Frequency", "kilohertz", "kHz");
        public static IotUnit megahertz => new IotUnit("Frequency", "megahertz", "MHz");
        public static IotUnit per_hour => new IotUnit("Frequency", "per_hour", "ph");

        // General Units
        public static IotUnit decibels_a => new IotUnit("General", "decibels_a", "dBA");
        public static IotUnit grams_per_square_meter => new IotUnit("General", "grams_per_square_meter", "g/m²");
        public static IotUnit nephelometric_turbidity_unit => new IotUnit("General", "nephelometric_turbidity_unit", "NTU");
        public static IotUnit pH => new IotUnit("General", "pH", "pH");

        // Humidity Units
        public static IotUnit grams_of_water_per_kilogram_dry_air => new IotUnit("Humidity", "grams_of_water_per_kilogram_dry_air", "g/kg");
        public static IotUnit percent_relative_humidity => new IotUnit("Humidity", "percent_relative_humidity", "%RH");

        // Illuminance Units
        public static IotUnit foot_candles => new IotUnit("Illuminance", "foot_candles", "fc");
        public static IotUnit lux => new IotUnit("Illuminance", "lux", "lx");

        // Inductance Units
        public static IotUnit henrys_inductance => new IotUnit("Inductance", "henrys", "H");
        public static IotUnit microhenrys_inductance => new IotUnit("Inductance", "microhenrys", "µH");
        public static IotUnit millihenrys_inductance => new IotUnit("Inductance", "millihenrys", "mH");

        // Length Units
        public static IotUnit centimeters => new IotUnit("Length", "centimeters", "cm");
        public static IotUnit feet => new IotUnit("Length", "feet", "ft");
        public static IotUnit inches => new IotUnit("Length", "inches", "in");
        public static IotUnit kilometers => new IotUnit("Length", "kilometers", "km");
        public static IotUnit meters => new IotUnit("Length", "meters", "m");
        public static IotUnit micrometers => new IotUnit("Length", "micrometers", "µm");
        public static IotUnit millimeters => new IotUnit("Length", "millimeters", "mm");

        // Light Units
        public static IotUnit candelas => new IotUnit("Light", "candelas", "cd");
        public static IotUnit candelas_per_square_meter => new IotUnit("Light", "candelas_per_square_meter", "cd/m²");
        public static IotUnit foot_candles_light => new IotUnit("Light", "foot_candles", "fc");
        public static IotUnit lumens => new IotUnit("Light", "lumens", "lm");
        public static IotUnit luxes => new IotUnit("Light", "luxes", "lx");
        public static IotUnit watts_per_square_foot => new IotUnit("Light", "watts_per_square_foot", "W/ft²");
        public static IotUnit watts_per_square_meter => new IotUnit("Light", "watts_per_square_meter", "W/m²");

        // Luminance Units
        public static IotUnit candelas_per_square_meter_luminance => new IotUnit("Luminance", "candelas_per_square_meter", "cd/m²");
        public static IotUnit nits => new IotUnit("Luminance", "nits", "nt");

        // Luminous Intensity Units
        public static IotUnit candela => new IotUnit("LuminousIntensity", "candela", "cd");

        // Magnetic Field Strength Units
        public static IotUnit amperes_per_meter_magnetic_field => new IotUnit("MagneticFieldStrength", "amperes_per_meter", "A/m");
        public static IotUnit oersteds => new IotUnit("MagneticFieldStrength", "oersteds", "Oe");

        // Magnetic Flux Units
        public static IotUnit maxwells => new IotUnit("MagneticFlux", "maxwells", "Mx");
        public static IotUnit webers => new IotUnit("MagneticFlux", "webers", "Wb");

        // Mass Units
        public static IotUnit grams => new IotUnit("Mass", "grams", "g");
        public static IotUnit kilograms => new IotUnit("Mass", "kilograms", "kg");
        public static IotUnit milligrams => new IotUnit("Mass", "milligrams", "mg");
        public static IotUnit pounds_mass => new IotUnit("Mass", "pounds_mass", "lb");
        public static IotUnit tons => new IotUnit("Mass", "tons", "t");

        // Mass Density Units
        public static IotUnit grams_per_cubic_centimeter => new IotUnit("MassDensity", "grams_per_cubic_centimeter", "g/cm³");
        public static IotUnit grams_per_cubic_meter => new IotUnit("MassDensity", "grams_per_cubic_meter", "g/m³");
        public static IotUnit kilograms_per_cubic_meter => new IotUnit("MassDensity", "kilograms_per_cubic_meter", "kg/m³");
        public static IotUnit micrograms_per_cubic_meter => new IotUnit("MassDensity", "micrograms_per_cubic_meter", "µg/m³");
        public static IotUnit milligrams_per_cubic_meter => new IotUnit("MassDensity", "milligrams_per_cubic_meter", "mg/m³");
        public static IotUnit nanograms_per_cubic_meter => new IotUnit("MassDensity", "nanograms_per_cubic_meter", "ng/m³");

        // Mass Flow Units
        public static IotUnit grams_per_minute => new IotUnit("MassFlow", "grams_per_minute", "g/min");
        public static IotUnit grams_per_second => new IotUnit("MassFlow", "grams_per_second", "g/s");
        public static IotUnit kilograms_per_hour => new IotUnit("MassFlow", "kilograms_per_hour", "kg/h");
        public static IotUnit kilograms_per_minute => new IotUnit("MassFlow", "kilograms_per_minute", "kg/min");
        public static IotUnit kilograms_per_second => new IotUnit("MassFlow", "kilograms_per_second", "kg/s");
        public static IotUnit pounds_mass_per_hour => new IotUnit("MassFlow", "pounds_mass_per_hour", "lb/h");
        public static IotUnit pounds_mass_per_minute => new IotUnit("MassFlow", "pounds_mass_per_minute", "lb/min");
        public static IotUnit pounds_mass_per_second => new IotUnit("MassFlow", "pounds_mass_per_second", "lb/s");
        public static IotUnit tons_per_hour => new IotUnit("MassFlow", "tons_per_hour", "t/h");

        // Mass Fraction Units
        public static IotUnit grams_per_gram => new IotUnit("MassFraction", "grams_per_gram", "g/g");
        public static IotUnit grams_per_kilogram => new IotUnit("MassFraction", "grams_per_kilogram", "g/kg");
        public static IotUnit grams_per_liter => new IotUnit("MassFraction", "grams_per_liter", "g/L");
        public static IotUnit grams_per_milliliter => new IotUnit("MassFraction", "grams_per_milliliter", "g/mL");
        public static IotUnit kilograms_per_kilogram => new IotUnit("MassFraction", "kilograms_per_kilogram", "kg/kg");
        public static IotUnit micrograms_per_liter => new IotUnit("MassFraction", "micrograms_per_liter", "µg/L");
        public static IotUnit milligrams_per_gram => new IotUnit("MassFraction", "milligrams_per_gram", "mg/g");
        public static IotUnit milligrams_per_kilogram => new IotUnit("MassFraction", "milligrams_per_kilogram", "mg/kg");
        public static IotUnit milligrams_per_liter => new IotUnit("MassFraction", "milligrams_per_liter", "mg/L");

        // Physical Properties Units
        public static IotUnit newton_seconds => new IotUnit("PhysicalProperties", "newton_seconds", "N·s");
        public static IotUnit newtons_per_meter => new IotUnit("PhysicalProperties", "newtons_per_meter", "N/m");
        public static IotUnit pascal_seconds => new IotUnit("PhysicalProperties", "pascal_seconds", "Pa·s");
        public static IotUnit square_meters_per_newton => new IotUnit("PhysicalProperties", "square_meters_per_newton", "m²/N");
        public static IotUnit watts_per_meter_per_degree_kelvin => new IotUnit("PhysicalProperties", "watts_per_meter_per_degree_kelvin", "W/(m·K)");
        public static IotUnit watts_per_square_meter_degree_kelvin => new IotUnit("PhysicalProperties", "watts_per_square_meter_degree_kelvin", "W/(m²·K)");

        // Power Units
        public static IotUnit horsepower => new IotUnit("Power", "horsepower", "hp");
        public static IotUnit joule_per_hours => new IotUnit("Power", "joule_per_hours", "J/h");
        public static IotUnit kilo_btus_per_hour => new IotUnit("Power", "kilo_btus_per_hour", "kBTU/h");
        public static IotUnit kilowatts => new IotUnit("Power", "kilowatts", "kW");
        public static IotUnit megawatts => new IotUnit("Power", "megawatts", "MW");
        public static IotUnit milliwatts => new IotUnit("Power", "milliwatts", "mW");
        public static IotUnit tons_refrigeration => new IotUnit("Power", "tons_refrigeration", "TR");
        public static IotUnit watts_power => new IotUnit("Power", "watts", "W");
        public static IotUnit btus_per_hour => new IotUnit("Power", "btus_per_hour", "BTU/h");

        // Pressure Units
        public static IotUnit bars_pressure => new IotUnit("Pressure", "bars", "bar");
        public static IotUnit centimeters_of_mercury => new IotUnit("Pressure", "centimeters_of_mercury", "cmHg");
        public static IotUnit centimeters_of_water => new IotUnit("Pressure", "centimeters_of_water", "cmH₂O");
        public static IotUnit hectopascals => new IotUnit("Pressure", "hectopascals", "hPa");
        public static IotUnit inches_of_mercury => new IotUnit("Pressure", "inches_of_mercury", "inHg");
        public static IotUnit inches_of_water => new IotUnit("Pressure", "inches_of_water", "inH₂O");
        public static IotUnit kilopascals => new IotUnit("Pressure", "kilopascals", "kPa");
        public static IotUnit millibars => new IotUnit("Pressure", "millibars", "mbar");
        public static IotUnit millimeters_of_mercury => new IotUnit("Pressure", "millimeters_of_mercury", "mmHg");
        public static IotUnit millimeters_of_water => new IotUnit("Pressure", "millimeters_of_water", "mmH₂O");
        public static IotUnit pascals => new IotUnit("Pressure", "pascals", "Pa");
        public static IotUnit pounds_force_per_square_inch => new IotUnit("Pressure", "pounds_force_per_square_inch", "psi");

        // Radiation Units
        public static IotUnit becquerels => new IotUnit("Radiation", "becquerels", "Bq");
        public static IotUnit curies => new IotUnit("Radiation", "curies", "Ci");
        public static IotUnit gray => new IotUnit("Radiation", "gray", "Gy");
        public static IotUnit kilobecquerels => new IotUnit("Radiation", "kilobecquerels", "kBq");
        public static IotUnit megabecquerels => new IotUnit("Radiation", "megabecquerels", "MBq");
        public static IotUnit milligray => new IotUnit("Radiation", "milligray", "mGy");
        public static IotUnit millirems => new IotUnit("Radiation", "millirems", "mrem");
        public static IotUnit millirems_per_hour => new IotUnit("Radiation", "millirems_per_hour", "mrem/h");
        public static IotUnit millisieverts => new IotUnit("Radiation", "millisieverts", "mSv");
        public static IotUnit microsieverts => new IotUnit("Radiation", "microsieverts", "µSv");
        public static IotUnit microsieverts_per_hour => new IotUnit("Radiation", "microsieverts_per_hour", "µSv/h");
        public static IotUnit microgray => new IotUnit("Radiation", "microgray", "µGy");
        public static IotUnit rads => new IotUnit("Radiation", "rads", "rad");
        public static IotUnit rems => new IotUnit("Radiation", "rems", "rem");
        public static IotUnit sieverts => new IotUnit("Radiation", "sieverts", "Sv");

        // Radiant Intensity Units
        public static IotUnit microwatts_per_steradian => new IotUnit("RadiantIntensity", "microwatts_per_steradian", "µW/sr");
        public static IotUnit watts_per_steradian => new IotUnit("RadiantIntensity", "watts_per_steradian", "W/sr");

        // Temperature Units
        public static IotUnit degree_days_celsius => new IotUnit("Temperature", "degree_days_celsius", "°C·d");
        public static IotUnit degree_days_fahrenheit => new IotUnit("Temperature", "degree_days_fahrenheit", "°F·d");
        public static IotUnit degrees_celsius => new IotUnit("Temperature", "degrees_celsius", "°C");
        public static IotUnit degrees_fahrenheit => new IotUnit("Temperature", "degrees_fahrenheit", "°F");
        public static IotUnit degrees_kelvin => new IotUnit("Temperature", "degrees_kelvin", "K");
        public static IotUnit degrees_rankine => new IotUnit("Temperature", "degrees_rankine", "°R");
        public static IotUnit delta_degrees_fahrenheit => new IotUnit("Temperature", "delta_degrees_fahrenheit", "∆°F");
        public static IotUnit delta_degrees_kelvin => new IotUnit("Temperature", "delta_degrees_kelvin", "∆K");

        // Temperature Rate Units
        public static IotUnit degrees_celsius_per_hour => new IotUnit("TemperatureRate", "degrees_celsius_per_hour", "°C/h");
        public static IotUnit degrees_celsius_per_minute => new IotUnit("TemperatureRate", "degrees_celsius_per_minute", "°C/min");
        public static IotUnit degrees_fahrenheit_per_hour => new IotUnit("TemperatureRate", "degrees_fahrenheit_per_hour", "°F/h");
        public static IotUnit degrees_fahrenheit_per_minute => new IotUnit("TemperatureRate", "degrees_fahrenheit_per_minute", "°F/min");
        public static IotUnit minutes_per_degree_kelvin => new IotUnit("TemperatureRate", "minutes_per_degree_kelvin", "min/K");
        public static IotUnit psi_per_degree_fahrenheit => new IotUnit("TemperatureRate", "psi_per_degree_fahrenheit", "psi/°F");

        // Time Units
        public static IotUnit days => new IotUnit("Time", "days", "d");
        public static IotUnit hours => new IotUnit("Time", "hours", "h");
        public static IotUnit hundredths_seconds => new IotUnit("Time", "hundredths_seconds", "hs");
        public static IotUnit milliseconds => new IotUnit("Time", "milliseconds", "ms");
        public static IotUnit minutes => new IotUnit("Time", "minutes", "min");
        public static IotUnit months => new IotUnit("Time", "months", "mo");
        public static IotUnit seconds => new IotUnit("Time", "seconds", "s");
        public static IotUnit weeks => new IotUnit("Time", "weeks", "wk");
        public static IotUnit years => new IotUnit("Time", "years", "yr");

        // Torque Units
        public static IotUnit newton_meters => new IotUnit("Torque", "newton_meters", "N·m");

        // Velocity Units
        public static IotUnit feet_per_minute => new IotUnit("Velocity", "feet_per_minute", "ft/min");
        public static IotUnit feet_per_second => new IotUnit("Velocity", "feet_per_second", "ft/s");
        public static IotUnit millimeters_per_second => new IotUnit("Velocity", "millimeters_per_second", "mm/s");
        public static IotUnit kilometers_per_hour => new IotUnit("Velocity", "kilometers_per_hour", "km/h");
        public static IotUnit meters_per_hour => new IotUnit("Velocity", "meters_per_hour", "m/h");
        public static IotUnit meters_per_minute => new IotUnit("Velocity", "meters_per_minute", "m/min");
        public static IotUnit meters_per_second => new IotUnit("Velocity", "meters_per_second", "m/s");
        public static IotUnit miles_per_hour => new IotUnit("Velocity", "miles_per_hour", "mph");
        public static IotUnit millimeters_per_minute => new IotUnit("Velocity", "millimeters_per_minute", "mm/min");

        // Volume Units
        public static IotUnit cubic_feet => new IotUnit("Volume", "cubic_feet", "ft³");
        public static IotUnit cubic_meters => new IotUnit("Volume", "cubic_meters", "m³");
        public static IotUnit imperial_gallons => new IotUnit("Volume", "imperial_gallons", "gal (imp)");
        public static IotUnit liters => new IotUnit("Volume", "liters", "L");
        public static IotUnit milliliters => new IotUnit("Volume", "milliliters", "mL");
        public static IotUnit us_gallons => new IotUnit("Volume", "us_gallons", "gal (US)");

        // Volume Specific Units
        public static IotUnit cubic_feet_per_pound => new IotUnit("VolumeSpecific", "cubic_feet_per_pound", "ft³/lb");
        public static IotUnit cubic_meters_per_kilogram => new IotUnit("VolumeSpecific", "cubic_meters_per_kilogram", "m³/kg");

        // Volumetric Flow Units
        public static IotUnit cubic_feet_per_day => new IotUnit("VolumetricFlow", "cubic_feet_per_day", "ft³/d");
        public static IotUnit cubic_feet_per_hour => new IotUnit("VolumetricFlow", "cubic_feet_per_hour", "ft³/h");
        public static IotUnit cubic_feet_per_minute => new IotUnit("VolumetricFlow", "cubic_feet_per_minute", "cfm");
        public static IotUnit cubic_feet_per_second => new IotUnit("VolumetricFlow", "cubic_feet_per_second", "cfs");
        public static IotUnit cubic_meters_per_day => new IotUnit("VolumetricFlow", "cubic_meters_per_day", "m³/d");
        public static IotUnit cubic_meters_per_hour => new IotUnit("VolumetricFlow", "cubic_meters_per_hour", "m³/h");
        public static IotUnit cubic_meters_per_minute => new IotUnit("VolumetricFlow", "cubic_meters_per_minute", "m³/min");
        public static IotUnit cubic_meters_per_second => new IotUnit("VolumetricFlow", "cubic_meters_per_second", "m³/s");
        public static IotUnit imperial_gallons_per_minute => new IotUnit("VolumetricFlow", "imperial_gallons_per_minute", "gal (imp)/min");
        public static IotUnit liters_per_hour => new IotUnit("VolumetricFlow", "liters_per_hour", "L/h");
        public static IotUnit liters_per_minute => new IotUnit("VolumetricFlow", "liters_per_minute", "L/min");
        public static IotUnit liters_per_second => new IotUnit("VolumetricFlow", "liters_per_second", "L/s");
        public static IotUnit milliliters_per_second => new IotUnit("VolumetricFlow", "milliliters_per_second", "mL/s");
        public static IotUnit million_standard_cubic_feet_per_day => new IotUnit("VolumetricFlow", "million_standard_cubic_feet_per_day", "MMscfd");
        public static IotUnit million_standard_cubic_feet_per_minute => new IotUnit("VolumetricFlow", "million_standard_cubic_feet_per_minute", "MMscfm");
        public static IotUnit pounds_mass_per_day => new IotUnit("VolumetricFlow", "pounds_mass_per_day", "lb/d");
        public static IotUnit standard_cubic_feet_per_day => new IotUnit("VolumetricFlow", "standard_cubic_feet_per_day", "scfd");
        public static IotUnit thousand_cubic_feet_per_day => new IotUnit("VolumetricFlow", "thousand_cubic_feet_per_day", "Mscfd");
        public static IotUnit thousand_standard_cubic_feet_per_day => new IotUnit("VolumetricFlow", "thousand_standard_cubic_feet_per_day", "Mscfd");
        public static IotUnit us_gallons_per_hour => new IotUnit("VolumetricFlow", "us_gallons_per_hour", "gal (US)/h");
        public static IotUnit us_gallons_per_minute => new IotUnit("VolumetricFlow", "us_gallons_per_minute", "gpm");

    }

    // Acceleration Category
    public struct Acceleration
    {
        public static IotUnit meters_per_second_per_second => All.meters_per_second_per_second;
        public static IotUnit standard_gravity => All.standard_gravity;
    }

    // Angular Category
    public struct Angular
    {
        public static IotUnit degrees_angular => All.degrees_angular;
        public static IotUnit radians => All.radians;
        public static IotUnit radians_per_second => All.radians_per_second;
        public static IotUnit revolutions_per_minute => All.revolutions_per_minute;
    }

    // Area Category
    public struct Area
    {
        public static IotUnit square_centimeters => All.square_centimeters;
        public static IotUnit square_feet => All.square_feet;
        public static IotUnit square_inches => All.square_inches;
        public static IotUnit square_meters => All.square_meters;
    }

    // Capacitance Category
    public struct Capacitance
    {
        public static IotUnit farads => All.farads;
        public static IotUnit microfarads => All.microfarads;
        public static IotUnit nanofarads => All.nanofarads;
        public static IotUnit picofarads => All.picofarads;
    }

    // Concentration Category
    public struct Concentration
    {
        public static IotUnit mole_percent => All.mole_percent;
        public static IotUnit parts_per_billion => All.parts_per_billion;
        public static IotUnit parts_per_million => All.parts_per_million;
        public static IotUnit percent => All.percent;
        public static IotUnit percent_obscuration_per_foot => All.percent_obscuration_per_foot;
        public static IotUnit percent_obscuration_per_meter => All.percent_obscuration_per_meter;
        public static IotUnit percent_per_second => All.percent_per_second;
        public static IotUnit per_mille => All.per_mille;
    }

    // Currency Category
    public struct Currency
    {
        public static IotUnit afghan_afghani => All.afghan_afghani;
        public static IotUnit albanian_lek => All.albanian_lek;
        public static IotUnit algerian_dinar => All.algerian_dinar;
        public static IotUnit angolan_kwanza => All.angolan_kwanza;
        public static IotUnit argentine_peso => All.argentine_peso;
        public static IotUnit armenian_dram => All.armenian_dram;
        public static IotUnit aruban_florin => All.aruban_florin;
        public static IotUnit australian_dollar => All.australian_dollar;
        public static IotUnit azerbaijani_manat => All.azerbaijani_manat;
        public static IotUnit bahamian_dollar => All.bahamian_dollar;
        public static IotUnit bahraini_dinar => All.bahraini_dinar;
        public static IotUnit bangladeshi_taka => All.bangladeshi_taka;
        public static IotUnit barbadian_dollar => All.barbadian_dollar;
        public static IotUnit belarusian_ruble => All.belarusian_ruble;
        public static IotUnit belize_dollar => All.belize_dollar;
        public static IotUnit bermudian_dollar => All.bermudian_dollar;
        public static IotUnit bhutanese_ngultrum => All.bhutanese_ngultrum;
        public static IotUnit bolivian_boliviano => All.bolivian_boliviano;
        public static IotUnit bosnia_and_herzegovina_convertible_mark => All.bosnia_and_herzegovina_convertible_mark;
        public static IotUnit botswana_pula => All.botswana_pula;
        public static IotUnit brazilian_real => All.brazilian_real;
        public static IotUnit brunei_dollar => All.brunei_dollar;
        public static IotUnit bulgarian_lev => All.bulgarian_lev;
        public static IotUnit burundian_franc => All.burundian_franc;
        public static IotUnit cape_verdean_escudo => All.cape_verdean_escudo;
        public static IotUnit cambodian_riel => All.cambodian_riel;
        public static IotUnit canadian_dollar => All.canadian_dollar;
        public static IotUnit cayman_islands_dollar => All.cayman_islands_dollar;
        public static IotUnit central_african_cfa_franc => All.central_african_cfa_franc;
        public static IotUnit chilean_peso => All.chilean_peso;
        public static IotUnit chinese_yuan => All.chinese_yuan;
        public static IotUnit colombian_peso => All.colombian_peso;
        public static IotUnit comorian_franc => All.comorian_franc;
        public static IotUnit congolese_franc => All.congolese_franc;
        public static IotUnit costa_rican_colon => All.costa_rican_colon;
        public static IotUnit croatian_kuna => All.croatian_kuna;
        public static IotUnit cuban_convertible_peso => All.cuban_convertible_peso;
        public static IotUnit cuban_peso => All.cuban_peso;
        public static IotUnit czech_koruna => All.czech_koruna;
        public static IotUnit danish_krone => All.danish_krone;
        public static IotUnit djiboutian_franc => All.djiboutian_franc;
        public static IotUnit dominican_peso => All.dominican_peso;
        public static IotUnit east_caribbean_dollar => All.east_caribbean_dollar;
        public static IotUnit egyptian_pound => All.egyptian_pound;
        public static IotUnit eritrean_nakfa => All.eritrean_nakfa;
        public static IotUnit ethiopian_birr => All.ethiopian_birr;
        public static IotUnit euro => All.euro;
        public static IotUnit falkland_islands_pound => All.falkland_islands_pound;
        public static IotUnit fiji_dollar => All.fiji_dollar;
        public static IotUnit gambian_dalasi => All.gambian_dalasi;
        public static IotUnit georgian_lari => All.georgian_lari;
        public static IotUnit ghanaian_cedi => All.ghanaian_cedi;
        public static IotUnit gibraltar_pound => All.gibraltar_pound;
        public static IotUnit guatemalan_quetzal => All.guatemalan_quetzal;
        public static IotUnit guinean_franc => All.guinean_franc;
        public static IotUnit guyanese_dollar => All.guyanese_dollar;
        public static IotUnit haitian_gourde => All.haitian_gourde;
        public static IotUnit honduran_lempira => All.honduran_lempira;
        public static IotUnit hong_kong_dollar => All.hong_kong_dollar;
        public static IotUnit hungarian_forint => All.hungarian_forint;
        public static IotUnit icelandic_krona => All.icelandic_krona;
        public static IotUnit indian_rupee => All.indian_rupee;
        public static IotUnit indonesian_rupiah => All.indonesian_rupiah;
        public static IotUnit iranian_rial => All.iranian_rial;
        public static IotUnit iraqi_dinar => All.iraqi_dinar;
        public static IotUnit israeli_new_shekel => All.israeli_new_shekel;
        public static IotUnit jamaican_dollar => All.jamaican_dollar;
        public static IotUnit japanese_yen => All.japanese_yen;
        public static IotUnit jordanian_dinar => All.jordanian_dinar;
        public static IotUnit kazakhstani_tenge => All.kazakhstani_tenge;
        public static IotUnit kenyan_shilling => All.kenyan_shilling;
        public static IotUnit kuwaiti_dinar => All.kuwaiti_dinar;
        public static IotUnit kyrgyzstani_som => All.kyrgyzstani_som;
        public static IotUnit lao_kip => All.lao_kip;
        public static IotUnit lebanese_pound => All.lebanese_pound;
        public static IotUnit lesotho_loti => All.lesotho_loti;
        public static IotUnit liberian_dollar => All.liberian_dollar;
        public static IotUnit libyan_dinar => All.libyan_dinar;
        public static IotUnit macanese_pataca => All.macanese_pataca;
        public static IotUnit malagasy_ariary => All.malagasy_ariary;
        public static IotUnit malawian_kwacha => All.malawian_kwacha;
        public static IotUnit malaysian_ringgit => All.malaysian_ringgit;
        public static IotUnit maldivian_rufiyaa => All.maldivian_rufiyaa;
        public static IotUnit mauritanian_ouguiya => All.mauritanian_ouguiya;
        public static IotUnit mauritian_rupee => All.mauritian_rupee;
        public static IotUnit mexican_peso => All.mexican_peso;
        public static IotUnit moldovan_leu => All.moldovan_leu;
        public static IotUnit mongolian_togrog => All.mongolian_togrog;
        public static IotUnit moroccan_dirham => All.moroccan_dirham;
        public static IotUnit mozambican_metical => All.mozambican_metical;
        public static IotUnit myanmar_kyat => All.myanmar_kyat;
        public static IotUnit namibian_dollar => All.namibian_dollar;
        public static IotUnit nepalese_rupee => All.nepalese_rupee;
        public static IotUnit netherlands_antillean_guilder => All.netherlands_antillean_guilder;
        public static IotUnit new_taiwan_dollar => All.new_taiwan_dollar;
        public static IotUnit new_zealand_dollar => All.new_zealand_dollar;
        public static IotUnit nicaraguan_cordoba => All.nicaraguan_cordoba;
        public static IotUnit nigerian_naira => All.nigerian_naira;
        public static IotUnit north_korean_won => All.north_korean_won;
        public static IotUnit norwegian_krone => All.norwegian_krone;
        public static IotUnit omani_rial => All.omani_rial;
        public static IotUnit pakistani_rupee => All.pakistani_rupee;
        public static IotUnit panamanian_balboa => All.panamanian_balboa;
        public static IotUnit papua_new_guinean_kina => All.papua_new_guinean_kina;
        public static IotUnit paraguayan_guarani => All.paraguayan_guarani;
        public static IotUnit peruvian_sol => All.peruvian_sol;
        public static IotUnit philippine_peso => All.philippine_peso;
        public static IotUnit polish_zloty => All.polish_zloty;
        public static IotUnit qatar_riyal => All.qatar_riyal;
        public static IotUnit romanian_leu => All.romanian_leu;
        public static IotUnit russian_ruble => All.russian_ruble;
        public static IotUnit rwandan_franc => All.rwandan_franc;
        public static IotUnit saint_helena_pound => All.saint_helena_pound;
        public static IotUnit samoan_tala => All.samoan_tala;
        public static IotUnit saudi_riyal => All.saudi_riyal;
        public static IotUnit serbian_dinar => All.serbian_dinar;
        public static IotUnit seychellois_rupee => All.seychellois_rupee;
        public static IotUnit sierra_leonean_leone => All.sierra_leonean_leone;
        public static IotUnit singapore_dollar => All.singapore_dollar;
        public static IotUnit solomon_islands_dollar => All.solomon_islands_dollar;
        public static IotUnit somali_shilling => All.somali_shilling;
        public static IotUnit south_african_rand => All.south_african_rand;
        public static IotUnit south_korean_won => All.south_korean_won;
        public static IotUnit south_sudanese_pound => All.south_sudanese_pound;
        public static IotUnit sri_lankan_rupee => All.sri_lankan_rupee;
        public static IotUnit sudanese_pound => All.sudanese_pound;
        public static IotUnit surinamese_dollar => All.surinamese_dollar;
        public static IotUnit swazi_lilangeni => All.swazi_lilangeni;
        public static IotUnit swedish_krona => All.swedish_krona;
        public static IotUnit swiss_franc => All.swiss_franc;
        public static IotUnit syrian_pound => All.syrian_pound;
        public static IotUnit taiwanese_dollar => All.taiwanese_dollar;
        public static IotUnit tajikistani_somoni => All.tajikistani_somoni;
        public static IotUnit tanzanian_shilling => All.tanzanian_shilling;
        public static IotUnit thai_baht => All.thai_baht;
        public static IotUnit tonga_paanga => All.tonga_paanga;
        public static IotUnit trinidad_and_tobago_dollar => All.trinidad_and_tobago_dollar;
        public static IotUnit tunisian_dinar => All.tunisian_dinar;
        public static IotUnit turkish_lira => All.turkish_lira;
        public static IotUnit turkmenistani_manat => All.turkmenistani_manat;
        public static IotUnit ugandan_shilling => All.ugandan_shilling;
        public static IotUnit ukrainian_hryvnia => All.ukrainian_hryvnia;
        public static IotUnit united_arab_emirates_dirham => All.united_arab_emirates_dirham;
        public static IotUnit uruguayan_peso => All.uruguayan_peso;
        public static IotUnit uzbekistani_som => All.uzbekistani_som;
        public static IotUnit vanuatu_vatu => All.vanuatu_vatu;
        public static IotUnit venezuelan_bolivar => All.venezuelan_bolivar;
        public static IotUnit vietnamese_dong => All.vietnamese_dong;
        public static IotUnit yemeni_rial => All.yemeni_rial;
        public static IotUnit zambian_kwacha => All.zambian_kwacha;
        public static IotUnit zimbabwean_dollar => All.zimbabwean_dollar;
    }

    // DataRate Category
    public struct DataRate
    {
        public static IotUnit bits_per_second => All.bits_per_second;
        public static IotUnit gigabits_per_second => All.gigabits_per_second;
        public static IotUnit kilobits_per_second => All.kilobits_per_second;
        public static IotUnit megabits_per_second => All.megabits_per_second;
    }

    // DataStorage Category
    public struct DataStorage
    {
        public static IotUnit bytes => All.bytes;
        public static IotUnit exabytes => All.exabytes;
        public static IotUnit gigabytes => All.gigabytes;
        public static IotUnit kilobytes => All.kilobytes;
        public static IotUnit megabytes => All.megabytes;
        public static IotUnit petabytes => All.petabytes;
        public static IotUnit terabytes => All.terabytes;
        public static IotUnit yottabytes => All.yottabytes;
        public static IotUnit zettabytes => All.zettabytes;
    }

    // ElectricCharge Category
    public struct ElectricCharge
    {
        public static IotUnit ampere_hours => All.ampere_hours;
        public static IotUnit coulombs => All.coulombs;
    }

    // ElectricPotential Category
    public struct ElectricPotential
    {
        public static IotUnit kilovolts => All.kilovolts;
        public static IotUnit millivolts => All.millivolts;
        public static IotUnit volts => All.volts;
    }

    // ElectricResistance Category
    public struct ElectricResistance
    {
        public static IotUnit kilohms => All.kilohms;
        public static IotUnit megohms => All.megohms;
        public static IotUnit milliohms => All.milliohms;
        public static IotUnit ohms => All.ohms;
    }

    // Electrical Category
    public struct Electrical
    {
        public static IotUnit amperes => All.amperes;
        public static IotUnit amperes_per_meter => All.amperes_per_meter;
        public static IotUnit amperes_per_square_meter => All.amperes_per_square_meter;
        public static IotUnit ampere_square_meters => All.ampere_square_meters;
        public static IotUnit bars => All.bars;
        public static IotUnit decibels => All.decibels;
        public static IotUnit decibels_millivolt => All.decibels_millivolt;
        public static IotUnit decibels_volt => All.decibels_volt;
        public static IotUnit degrees_phase => All.degrees_phase;
        public static IotUnit henrys => All.henrys;
        public static IotUnit kilovolt_amperes => All.kilovolt_amperes;
        public static IotUnit kilovolt_amperes_reactive => All.kilovolt_amperes_reactive;
        public static IotUnit megavolt_amperes => All.megavolt_amperes;
        public static IotUnit megavolt_amperes_reactive => All.megavolt_amperes_reactive;
        public static IotUnit microamperes => All.microamperes;
        public static IotUnit microhenrys => All.microhenrys;
        public static IotUnit milliamperes => All.milliamperes;
        public static IotUnit millihenrys => All.millihenrys;
        public static IotUnit millivolt_amperes => All.millivolt_amperes;
        public static IotUnit nanohenrys => All.nanohenrys;
        public static IotUnit siemens => All.siemens;
        public static IotUnit teslas => All.teslas;
        public static IotUnit volts_dc => All.volts_dc;
        public static IotUnit watts => All.watts;
        public static IotUnit watts_per_meter_kelvin => All.watts_per_meter_kelvin;
        public static IotUnit watt_seconds => All.watt_seconds;
        public static IotUnit watt_hours => All.watt_hours;
        public static IotUnit watt_per_square_meter => All.watt_per_square_meter;
    }

    // Energy Category
    public struct Energy
    {
        public static IotUnit ampere_seconds => All.ampere_seconds;
        public static IotUnit btus => All.btus;
        public static IotUnit joules => All.joules;
        public static IotUnit kilo_btus => All.kilo_btus;
        public static IotUnit kilojoules => All.kilojoules;
        public static IotUnit kilojoules_per_kilogram => All.kilojoules_per_kilogram;
        public static IotUnit kilovolt_ampere_hours => All.kilovolt_ampere_hours;
        public static IotUnit kilovolt_ampere_hours_reactive => All.kilovolt_ampere_hours_reactive;
        public static IotUnit kilowatt_hours => All.kilowatt_hours;
        public static IotUnit kilowatt_hours_reactive => All.kilowatt_hours_reactive;
        public static IotUnit megajoules => All.megajoules;
        public static IotUnit megavolt_ampere_hours => All.megavolt_ampere_hours;
        public static IotUnit megavolt_ampere_hours_reactive => All.megavolt_ampere_hours_reactive;
        public static IotUnit megawatt_hours => All.megawatt_hours;
        public static IotUnit megawatt_hours_reactive => All.megawatt_hours_reactive;
        public static IotUnit ton_hours => All.ton_hours;
        public static IotUnit volt_ampere_hours => All.volt_ampere_hours;
        public static IotUnit volt_ampere_hours_reactive => All.volt_ampere_hours_reactive;
        public static IotUnit volt_square_hours => All.volt_square_hours;
        public static IotUnit watt_hours => All.watt_hours;
        public static IotUnit watt_hours_reactive => All.watt_hours_reactive;
    }


    public struct EnergyDensity
    {
        public static IotUnit joules_per_cubic_meter => All.joules_per_cubic_meter;
        public static IotUnit kilowatt_hours_per_square_foot => All.kilowatt_hours_per_square_foot;
        public static IotUnit kilowatt_hours_per_square_meter => All.kilowatt_hours_per_square_meter;
        public static IotUnit megajoules_per_square_foot => All.megajoules_per_square_foot;
        public static IotUnit megajoules_per_square_meter => All.megajoules_per_square_meter;
        public static IotUnit watt_hours_per_cubic_meter => All.watt_hours_per_cubic_meter;
    }

    public struct EnergySpecific
    {
        public static IotUnit joule_seconds => All.joule_seconds;
    }

    public struct Enthalpy
    {
        public static IotUnit btus_per_pound => All.btus_per_pound;
        public static IotUnit btus_per_pound_dry_air => All.btus_per_pound_dry_air;
        public static IotUnit joules_per_degree_kelvin => All.joules_per_degree_kelvin;
        public static IotUnit joules_per_kilogram_dry_air => All.joules_per_kilogram_dry_air;
        public static IotUnit joules_per_kilogram_degree_kelvin => All.joules_per_kilogram_degree_kelvin;
        public static IotUnit kilojoules_per_degree_kelvin => All.kilojoules_per_degree_kelvin;
        public static IotUnit kilojoules_per_kilogram_dry_air => All.kilojoules_per_kilogram_dry_air;
        public static IotUnit megajoules_per_degree_kelvin => All.megajoules_per_degree_kelvin;
        public static IotUnit megajoules_per_kilogram_dry_air => All.megajoules_per_kilogram_dry_air;
    }


    public struct Force
    {
        public static IotUnit newton => All.newton;
    }

    public struct Frequency
    {
        public static IotUnit cycles_per_hour => All.cycles_per_hour;
        public static IotUnit cycles_per_minute => All.cycles_per_minute;
        public static IotUnit hertz => All.hertz;
        public static IotUnit kilohertz => All.kilohertz;
        public static IotUnit megahertz => All.megahertz;
        public static IotUnit per_hour => All.per_hour;
    }

    public struct General
    {
        public static IotUnit decibels_a => All.decibels_a;
        public static IotUnit grams_per_square_meter => All.grams_per_square_meter;
        public static IotUnit nephelometric_turbidity_unit => All.nephelometric_turbidity_unit;
        public static IotUnit pH => All.pH;
    }

    public struct Humidity
    {
        public static IotUnit grams_of_water_per_kilogram_dry_air => All.grams_of_water_per_kilogram_dry_air;
        public static IotUnit percent_relative_humidity => All.percent_relative_humidity;
    }

    public struct Illuminance
    {
        public static IotUnit foot_candles => All.foot_candles;
        public static IotUnit lux => All.lux;
    }

    public struct Inductance
    {
        public static IotUnit henrys => All.henrys;
        public static IotUnit microhenrys => All.microhenrys;
        public static IotUnit millihenrys => All.millihenrys;
    }

    public struct Length
    {
        public static IotUnit centimeters => All.centimeters;
        public static IotUnit feet => All.feet;
        public static IotUnit inches => All.inches;
        public static IotUnit kilometers => All.kilometers;
        public static IotUnit meters => All.meters;
        public static IotUnit micrometers => All.micrometers;
        public static IotUnit millimeters => All.millimeters;
    }

    public struct Light
    {
        public static IotUnit candelas => All.candelas;
        public static IotUnit candelas_per_square_meter => All.candelas_per_square_meter;
        public static IotUnit foot_candles_light => All.foot_candles_light;
        public static IotUnit lumens => All.lumens;
        public static IotUnit luxes => All.luxes;
        public static IotUnit watts_per_square_foot => All.watts_per_square_foot;
        public static IotUnit watts_per_square_meter => All.watts_per_square_meter;
    }

    public struct Luminance
    {
        public static IotUnit candelas_per_square_meter_luminance => All.candelas_per_square_meter_luminance;
        public static IotUnit nits => All.nits;
    }

    public struct LuminousIntensity
    {
        public static IotUnit candela => All.candela;
    }

    public struct MagneticFieldStrength
    {
        public static IotUnit amperes_per_meter => All.amperes_per_meter;
        public static IotUnit oersteds => All.oersteds;
    }

    public struct MagneticFlux
    {
        public static IotUnit maxwells => All.maxwells;
        public static IotUnit webers => All.webers;
    }

    public struct Mass
    {
        public static IotUnit grams => All.grams;
        public static IotUnit kilograms => All.kilograms;
        public static IotUnit milligrams => All.milligrams;
        public static IotUnit pounds_mass => All.pounds_mass;
        public static IotUnit tons => All.tons;
    }

    public struct MassDensity
    {
        public static IotUnit grams_per_cubic_centimeter => All.grams_per_cubic_centimeter;
        public static IotUnit grams_per_cubic_meter => All.grams_per_cubic_meter;
        public static IotUnit kilograms_per_cubic_meter => All.kilograms_per_cubic_meter;
        public static IotUnit micrograms_per_cubic_meter => All.micrograms_per_cubic_meter;
        public static IotUnit milligrams_per_cubic_meter => All.milligrams_per_cubic_meter;
        public static IotUnit nanograms_per_cubic_meter => All.nanograms_per_cubic_meter;
    }

    public struct MassFlow
    {
        public static IotUnit grams_per_minute => All.grams_per_minute;
        public static IotUnit grams_per_second => All.grams_per_second;
        public static IotUnit kilograms_per_hour => All.kilograms_per_hour;
        public static IotUnit kilograms_per_minute => All.kilograms_per_minute;
        public static IotUnit kilograms_per_second => All.kilograms_per_second;
        public static IotUnit pounds_mass_per_hour => All.pounds_mass_per_hour;
        public static IotUnit pounds_mass_per_minute => All.pounds_mass_per_minute;
        public static IotUnit pounds_mass_per_second => All.pounds_mass_per_second;
        public static IotUnit tons_per_hour => All.tons_per_hour;
    }

    public struct MassFraction
    {
        public static IotUnit grams_per_gram => All.grams_per_gram;
        public static IotUnit grams_per_kilogram => All.grams_per_kilogram;
        public static IotUnit grams_per_liter => All.grams_per_liter;
        public static IotUnit grams_per_milliliter => All.grams_per_milliliter;
        public static IotUnit kilograms_per_kilogram => All.kilograms_per_kilogram;
        public static IotUnit micrograms_per_liter => All.micrograms_per_liter;
        public static IotUnit milligrams_per_gram => All.milligrams_per_gram;
        public static IotUnit milligrams_per_kilogram => All.milligrams_per_kilogram;
        public static IotUnit milligrams_per_liter => All.milligrams_per_liter;
    }

    public struct PhysicalProperties
    {
        public static IotUnit newton_seconds => All.newton_seconds;
        public static IotUnit newtons_per_meter => All.newtons_per_meter;
        public static IotUnit pascal_seconds => All.pascal_seconds;
        public static IotUnit square_meters_per_newton => All.square_meters_per_newton;
        public static IotUnit watts_per_meter_per_degree_kelvin => All.watts_per_meter_per_degree_kelvin;
        public static IotUnit watts_per_square_meter_degree_kelvin => All.watts_per_square_meter_degree_kelvin;
    }

    public struct Power
    {
        public static IotUnit horsepower => All.horsepower;
        public static IotUnit joule_per_hours => All.joule_per_hours;
        public static IotUnit kilo_btus_per_hour => All.kilo_btus_per_hour;
        public static IotUnit kilowatts => All.kilowatts;
        public static IotUnit megawatts => All.megawatts;
        public static IotUnit milliwatts => All.milliwatts;
        public static IotUnit tons_refrigeration => All.tons_refrigeration;
        public static IotUnit watts => All.watts;
        public static IotUnit btus_per_hour => All.btus_per_hour;
    }

    public struct Pressure
    {
        public static IotUnit bars => All.bars;
        public static IotUnit centimeters_of_mercury => All.centimeters_of_mercury;
        public static IotUnit centimeters_of_water => All.centimeters_of_water;
        public static IotUnit hectopascals => All.hectopascals;
        public static IotUnit inches_of_mercury => All.inches_of_mercury;
        public static IotUnit inches_of_water => All.inches_of_water;
        public static IotUnit kilopascals => All.kilopascals;
        public static IotUnit millibars => All.millibars;
        public static IotUnit millimeters_of_mercury => All.millimeters_of_mercury;
        public static IotUnit millimeters_of_water => All.millimeters_of_water;
        public static IotUnit pascals => All.pascals;
        public static IotUnit pounds_force_per_square_inch => All.pounds_force_per_square_inch;
    }

    public struct Radiation
    {
        public static IotUnit becquerels => All.becquerels;
        public static IotUnit curies => All.curies;
        public static IotUnit gray => All.gray;
        public static IotUnit kilobecquerels => All.kilobecquerels;
        public static IotUnit megabecquerels => All.megabecquerels;
        public static IotUnit milligray => All.milligray;
        public static IotUnit millirems => All.millirems;
        public static IotUnit millirems_per_hour => All.millirems_per_hour;
        public static IotUnit millisieverts => All.millisieverts;
        public static IotUnit microsieverts => All.microsieverts;
        public static IotUnit microsieverts_per_hour => All.microsieverts_per_hour;
        public static IotUnit microgray => All.microgray;
        public static IotUnit rads => All.rads;
        public static IotUnit rems => All.rems;
        public static IotUnit sieverts => All.sieverts;
    }

    public struct RadiantIntensity
    {
        public static IotUnit microwatts_per_steradian => All.microwatts_per_steradian;
        public static IotUnit watts_per_steradian => All.watts_per_steradian;
    }

    public struct Temperature
    {
        public static IotUnit degree_days_celsius => All.degree_days_celsius;
        public static IotUnit degree_days_fahrenheit => All.degree_days_fahrenheit;
        public static IotUnit degrees_celsius => All.degrees_celsius;
        public static IotUnit degrees_fahrenheit => All.degrees_fahrenheit;
        public static IotUnit degrees_kelvin => All.degrees_kelvin;
        public static IotUnit degrees_rankine => All.degrees_rankine;
        public static IotUnit delta_degrees_fahrenheit => All.delta_degrees_fahrenheit;
        public static IotUnit delta_degrees_kelvin => All.delta_degrees_kelvin;
    }

    public struct TemperatureRate
    {
        public static IotUnit degrees_celsius_per_hour => All.degrees_celsius_per_hour;
        public static IotUnit degrees_celsius_per_minute => All.degrees_celsius_per_minute;
        public static IotUnit degrees_fahrenheit_per_hour => All.degrees_fahrenheit_per_hour;
        public static IotUnit degrees_fahrenheit_per_minute => All.degrees_fahrenheit_per_minute;
        public static IotUnit minutes_per_degree_kelvin => All.minutes_per_degree_kelvin;
        public static IotUnit psi_per_degree_fahrenheit => All.psi_per_degree_fahrenheit;
    }

    public struct Time
    {
        public static IotUnit days => All.days;
        public static IotUnit hours => All.hours;
        public static IotUnit hundredths_seconds => All.hundredths_seconds;
        public static IotUnit milliseconds => All.milliseconds;
        public static IotUnit minutes => All.minutes;
        public static IotUnit months => All.months;
        public static IotUnit seconds => All.seconds;
        public static IotUnit weeks => All.weeks;
        public static IotUnit years => All.years;
    }

    public struct Torque
    {
        public static IotUnit newton_meters => All.newton_meters;
    }

    public struct Velocity
    {
        public static IotUnit feet_per_minute => All.feet_per_minute;
        public static IotUnit feet_per_second => All.feet_per_second;
        public static IotUnit millimeters_per_second => All.millimeters_per_second;
        public static IotUnit kilometers_per_hour => All.kilometers_per_hour;
        public static IotUnit meters_per_hour => All.meters_per_hour;
        public static IotUnit meters_per_minute => All.meters_per_minute;
        public static IotUnit meters_per_second => All.meters_per_second;
        public static IotUnit miles_per_hour => All.miles_per_hour;
        public static IotUnit millimeters_per_minute => All.millimeters_per_minute;
    }

    public struct Volume
    {
        public static IotUnit cubic_feet => All.cubic_feet;
        public static IotUnit cubic_meters => All.cubic_meters;
        public static IotUnit imperial_gallons => All.imperial_gallons;
        public static IotUnit liters => All.liters;
        public static IotUnit milliliters => All.milliliters;
        public static IotUnit us_gallons => All.us_gallons;
    }

    public struct VolumeSpecific
    {
        public static IotUnit cubic_feet_per_pound => All.cubic_feet_per_pound;
        public static IotUnit cubic_meters_per_kilogram => All.cubic_meters_per_kilogram;
    }

    public struct VolumetricFlow
    {
        public static IotUnit cubic_feet_per_day => All.cubic_feet_per_day;
        public static IotUnit cubic_feet_per_hour => All.cubic_feet_per_hour;
        public static IotUnit cubic_feet_per_minute => All.cubic_feet_per_minute;
        public static IotUnit cubic_feet_per_second => All.cubic_feet_per_second;
        public static IotUnit cubic_meters_per_day => All.cubic_meters_per_day;
        public static IotUnit cubic_meters_per_hour => All.cubic_meters_per_hour;
        public static IotUnit cubic_meters_per_minute => All.cubic_meters_per_minute;
        public static IotUnit cubic_meters_per_second => All.cubic_meters_per_second;
        public static IotUnit imperial_gallons_per_minute => All.imperial_gallons_per_minute;
        public static IotUnit liters_per_hour => All.liters_per_hour;
        public static IotUnit liters_per_minute => All.liters_per_minute;
        public static IotUnit liters_per_second => All.liters_per_second;
        public static IotUnit milliliters_per_second => All.milliliters_per_second;
        public static IotUnit million_standard_cubic_feet_per_day => All.million_standard_cubic_feet_per_day;
        public static IotUnit million_standard_cubic_feet_per_minute => All.million_standard_cubic_feet_per_minute;
        public static IotUnit pounds_mass_per_day => All.pounds_mass_per_day;
        public static IotUnit standard_cubic_feet_per_day => All.standard_cubic_feet_per_day;
        public static IotUnit thousand_cubic_feet_per_day => All.thousand_cubic_feet_per_day;
        public static IotUnit thousand_standard_cubic_feet_per_day => All.thousand_standard_cubic_feet_per_day;
        public static IotUnit us_gallons_per_hour => All.us_gallons_per_hour;
        public static IotUnit us_gallons_per_minute => All.us_gallons_per_minute;
    }

}

