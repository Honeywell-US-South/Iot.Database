namespace Iot.Database.IotValueUnits;


public partial struct Units
{
    public static IotUnit no_unit => new IotUnit("None", "no_unit", "", null);

    public struct All
    {
        // Acceleration
        public static IotUnit meters_per_second_per_second => new IotUnit("Acceleration", "meters_per_second_per_second", "m/s²",
            new Dictionary<string, string>
            {
                { "standard_gravity", "value * 9.80665" } // Convert g to m/s² (target: meters_per_second_per_second)
            }
        );
        public static IotUnit standard_gravity => new IotUnit("Acceleration", "standard_gravity", "g",
            new Dictionary<string, string>
            {
                { "meters_per_second_per_second", "value / 9.80665" } // Convert m/s² to g (target: standard_gravity)
            }
        );

        // Angular
        public static IotUnit degrees_angular => new IotUnit("Angular", "degrees_angular", "°",
            new Dictionary<string, string>
            {
                { "radians", "value * (180 / Math.PI)" }, // Convert radians to degrees (target: degrees_angular)
                { "radians_per_second", "value * (180 / Math.PI) * 60" }, // Convert rad/s to degrees (target: degrees_angular)
                { "revolutions_per_minute", "value * 6" } // Convert RPM to degrees (target: degrees_angular)
            }
        );
        public static IotUnit radians => new IotUnit("Angular", "radians", "rad",
            new Dictionary<string, string>
            {
                { "degrees_angular", "value * (Math.PI / 180)" }, // Convert degrees to radians (target: radians)
                { "radians_per_second", "value * 60" }, // Convert rad/s to radians (target: radians)
                { "revolutions_per_minute", "value * ((2 * Math.PI) / 60)" } // Convert RPM to radians (target: radians)
            }
        );
        public static IotUnit radians_per_second => new IotUnit("Angular", "radians_per_second", "rad/s",
            new Dictionary<string, string>
            {
                { "degrees_angular", "(value * (Math.PI / 180)) / (1 / 60)" }, // Convert degrees to rad/s (target: radians_per_second)
                { "radians", "value / 60" }, // Convert radians to rad/s (target: radians_per_second)
                { "revolutions_per_minute", "value * ((2 * Math.PI) / 60)" } // Convert RPM to rad/s (target: radians_per_second)
            }
        );
        public static IotUnit revolutions_per_minute => new IotUnit("Angular", "revolutions_per_minute", "RPM",
            new Dictionary<string, string>
            {
                { "degrees_angular", "value / 6" }, // Convert degrees to RPM (target: revolutions_per_minute)
                { "radians", "value / ((2 * Math.PI) / 60)" }, // Convert radians to RPM (target: revolutions_per_minute)
                { "radians_per_second", "value / ((2 * Math.PI) / 60)" } // Convert rad/s to RPM (target: revolutions_per_minute)
            }
        );

        // Area
        public static IotUnit square_centimeters => new IotUnit("Area", "square_centimeters", "cm²",
            new Dictionary<string, string>
            {
                { "square_meters", "value * 10000" }, // m² to cm²
                { "square_feet", "value * 929.0304" }, // ft² to cm²
                { "square_inches", "value * 6.4516" } // in² to cm²
            }
        );
        public static IotUnit square_feet => new IotUnit("Area", "square_feet", "ft²",
            new Dictionary<string, string>
            {
                { "square_meters", "value * 10.7639" }, // m² to ft²
                { "square_centimeters", "value / 929.0304" }, // cm² to ft²
                { "square_inches", "value / 144" } // in² to ft²
            }
        );
        public static IotUnit square_inches => new IotUnit("Area", "square_inches", "in²",
            new Dictionary<string, string>
            {
                { "square_meters", "value * 1550.0031" }, // m² to in²
                { "square_centimeters", "value / 6.4516" }, // cm² to in²
                { "square_feet", "value * 144" } // ft² to in²
            }
        );
        public static IotUnit square_meters => new IotUnit("Area", "square_meters", "m²",
            new Dictionary<string, string>
            {
                { "square_centimeters", "value / 10000" }, // cm² to m²
                { "square_feet", "value / 10.7639" }, // ft² to m²
                { "square_inches", "value / 1550.0031" } // in² to m²
            }
        );

        // Capacitance
        public static IotUnit farads => new IotUnit("Capacitance", "farads", "F",
            new Dictionary<string, string>
            {
                { "microfarads", "value * 1e6" }, // Convert μF to F (target: farads)
                { "nanofarads", "value * 1e9" }, // Convert nF to F (target: farads)
                { "picofarads", "value * 1e12" } // Convert pF to F (target: farads)
            }
        );
        public static IotUnit microfarads => new IotUnit("Capacitance", "microfarads", "μF",
            new Dictionary<string, string>
            {
                { "farads", "value / 1e6" }, // Convert F to μF (target: microfarads)
                { "nanofarads", "value * 1e3" }, // Convert nF to μF (target: microfarads)
                { "picofarads", "value * 1e6" } // Convert pF to μF (target: microfarads)
            }
        );
        public static IotUnit nanofarads => new IotUnit("Capacitance", "nanofarads", "nF",
            new Dictionary<string, string>
            {
                { "farads", "value / 1e9" }, // Convert F to nF (target: nanofarads)
                { "microfarads", "value / 1e3" }, // Convert μF to nF (target: nanofarads)
                { "picofarads", "value * 1e3" } // Convert pF to nF (target: nanofarads)
            }
        );
        public static IotUnit picofarads => new IotUnit("Capacitance", "picofarads", "pF",
            new Dictionary<string, string>
            {
                { "farads", "value / 1e12" }, // Convert F to pF (target: picofarads)
                { "microfarads", "value / 1e6" }, // Convert μF to pF (target: picofarads)
                { "nanofarads", "value / 1e3" } // Convert nF to pF (target: picofarads)
            }
        );

        // Concentration
        public static IotUnit mole_percent => new IotUnit("Concentration", "mole_percent", "mol%",
            new Dictionary<string, string>
            {
                { "percent", "value * 1" }, // Convert % to mol% (target: mole_percent)
                { "parts_per_million", "value * 10000" }, // Convert ppm to mol% (target: mole_percent)
                { "parts_per_billion", "value * 1e7" }, // Convert ppb to mol% (target: mole_percent)
                { "per_mille", "value * 10" } // Convert ‰ to mol% (target: mole_percent)
            }
        );

        public static IotUnit parts_per_billion => new IotUnit("Concentration", "parts_per_billion", "ppb",
            new Dictionary<string, string>
            {
                { "mole_percent", "value / 1e7" }, // Convert mol% to ppb (target: parts_per_billion)
                { "percent", "value * 1e7 / 100" }, // Convert % to ppb (target: parts_per_billion)
                { "parts_per_million", "value / 1000" }, // Convert ppm to ppb (target: parts_per_billion)
                { "per_mille", "value * 1e6 / 1000" } // Convert ‰ to ppb (target: parts_per_billion)
            }
        );

        public static IotUnit parts_per_million => new IotUnit("Concentration", "parts_per_million", "ppm",
            new Dictionary<string, string>
            {
                { "mole_percent", "value / 10000" }, // Convert mol% to ppm (target: parts_per_million)
                { "percent", "value * 1e6 / 100" }, // Convert % to ppm (target: parts_per_million)
                { "parts_per_billion", "value * 1000" }, // Convert ppb to ppm (target: parts_per_million)
                { "per_mille", "value * 1e5 / 1000" } // Convert ‰ to ppm (target: parts_per_million)
            }
        );

        public static IotUnit percent => new IotUnit("Concentration", "percent", "%",
            new Dictionary<string, string>
            {
                { "mole_percent", "value * 1" }, // Convert mol% to % (target: percent)
                { "parts_per_million", "value / (1e6 / 100)" }, // Convert ppm to % (target: percent)
                { "parts_per_billion", "value / (1e7 / 100)" }, // Convert ppb to % (target: percent)
                { "per_mille", "value / 10" } // Convert ‰ to % (target: percent)
            }
        );

        public static IotUnit percent_obscuration_per_foot => new IotUnit("Concentration", "percent_obscuration_per_foot", "%/ft",
            new Dictionary<string, string>
            {
                { "percent_obscuration_per_meter", "value / 3.28084" } // Convert %/m to %/ft (target: percent_obscuration_per_foot)
            }
        );

        public static IotUnit percent_obscuration_per_meter => new IotUnit("Concentration", "percent_obscuration_per_meter", "%/m",
            new Dictionary<string, string>
            {
                { "percent_obscuration_per_foot", "value * 3.28084" } // Convert %/ft to %/m (target: percent_obscuration_per_meter)
            }
        );

        public static IotUnit percent_per_second => new IotUnit("Concentration", "percent_per_second", "%/s",
            new Dictionary<string, string>
            {
                // No additional units defined for %/s yet
            }
        );

        public static IotUnit per_mille => new IotUnit("Concentration", "per_mille", "‰",
            new Dictionary<string, string>
            {
                { "mole_percent", "value / 10" }, // Convert mol% to ‰ (target: per_mille)
                { "parts_per_million", "value * 1000 / 1e5" }, // Convert ppm to ‰ (target: per_mille)
                { "parts_per_billion", "value * 1000 / 1e6" }, // Convert ppb to ‰ (target: per_mille)
                { "percent", "value * 10" } // Convert % to ‰ (target: per_mille)
            }
        );

        // Currency
        public static IotUnit afghan_afghani => new IotUnit("Currency", "afghan_afghani", "؋", null);
        public static IotUnit albanian_lek => new IotUnit("Currency", "albanian_lek", "L", null);
        public static IotUnit algerian_dinar => new IotUnit("Currency", "algerian_dinar", "د.ج", null);
        public static IotUnit angolan_kwanza => new IotUnit("Currency", "angolan_kwanza", "Kz", null);
        public static IotUnit argentine_peso => new IotUnit("Currency", "argentine_peso", "$", null);
        public static IotUnit armenian_dram => new IotUnit("Currency", "armenian_dram", "֏", null);
        public static IotUnit aruban_florin => new IotUnit("Currency", "aruban_florin", "ƒ", null);
        public static IotUnit australian_dollar => new IotUnit("Currency", "australian_dollar", "$", null);
        public static IotUnit azerbaijani_manat => new IotUnit("Currency", "azerbaijani_manat", "₼", null);
        public static IotUnit bahamian_dollar => new IotUnit("Currency", "bahamian_dollar", "$", null);
        public static IotUnit bahraini_dinar => new IotUnit("Currency", "bahraini_dinar", ".د.ب", null);
        public static IotUnit bangladeshi_taka => new IotUnit("Currency", "bangladeshi_taka", "৳", null);
        public static IotUnit barbadian_dollar => new IotUnit("Currency", "barbadian_dollar", "$", null);
        public static IotUnit belarusian_ruble => new IotUnit("Currency", "belarusian_ruble", "Br", null);
        public static IotUnit belize_dollar => new IotUnit("Currency", "belize_dollar", "$", null);
        public static IotUnit bermudian_dollar => new IotUnit("Currency", "bermudian_dollar", "$", null);
        public static IotUnit bhutanese_ngultrum => new IotUnit("Currency", "bhutanese_ngultrum", "Nu.", null);
        public static IotUnit bolivian_boliviano => new IotUnit("Currency", "bolivian_boliviano", "Bs.", null);
        public static IotUnit bosnia_and_herzegovina_convertible_mark => new IotUnit("Currency", "bosnia_and_herzegovina_convertible_mark", "KM", null);
        public static IotUnit botswana_pula => new IotUnit("Currency", "botswana_pula", "P", null);
        public static IotUnit brazilian_real => new IotUnit("Currency", "brazilian_real", "R$", null);
        public static IotUnit brunei_dollar => new IotUnit("Currency", "brunei_dollar", "$", null);
        public static IotUnit bulgarian_lev => new IotUnit("Currency", "bulgarian_lev", "лв", null);
        public static IotUnit burundian_franc => new IotUnit("Currency", "burundian_franc", "FBu", null);
        public static IotUnit cape_verdean_escudo => new IotUnit("Currency", "cape_verdean_escudo", "$", null);
        public static IotUnit cambodian_riel => new IotUnit("Currency", "cambodian_riel", "៛", null);
        public static IotUnit canadian_dollar => new IotUnit("Currency", "canadian_dollar", "$", null);
        public static IotUnit cayman_islands_dollar => new IotUnit("Currency", "cayman_islands_dollar", "$", null);
        public static IotUnit central_african_cfa_franc => new IotUnit("Currency", "central_african_cfa_franc", "Fr", null);
        public static IotUnit chilean_peso => new IotUnit("Currency", "chilean_peso", "$", null);
        public static IotUnit chinese_yuan => new IotUnit("Currency", "chinese_yuan", "¥", null);
        public static IotUnit colombian_peso => new IotUnit("Currency", "colombian_peso", "$", null);
        public static IotUnit comorian_franc => new IotUnit("Currency", "comorian_franc", "CF", null);
        public static IotUnit congolese_franc => new IotUnit("Currency", "congolese_franc", "FC", null);
        public static IotUnit costa_rican_colon => new IotUnit("Currency", "costa_rican_colon", "₡", null);
        public static IotUnit croatian_kuna => new IotUnit("Currency", "croatian_kuna", "kn", null);
        public static IotUnit cuban_convertible_peso => new IotUnit("Currency", "cuban_convertible_peso", "$", null);
        public static IotUnit cuban_peso => new IotUnit("Currency", "cuban_peso", "$", null);
        public static IotUnit czech_koruna => new IotUnit("Currency", "czech_koruna", "Kč", null);
        public static IotUnit danish_krone => new IotUnit("Currency", "danish_krone", "kr", null);
        public static IotUnit djiboutian_franc => new IotUnit("Currency", "djiboutian_franc", "Fdj", null);
        public static IotUnit dominican_peso => new IotUnit("Currency", "dominican_peso", "$", null);
        public static IotUnit east_caribbean_dollar => new IotUnit("Currency", "east_caribbean_dollar", "$", null);
        public static IotUnit egyptian_pound => new IotUnit("Currency", "egyptian_pound", "£", null);
        public static IotUnit eritrean_nakfa => new IotUnit("Currency", "eritrean_nakfa", "Nfk", null);
        public static IotUnit ethiopian_birr => new IotUnit("Currency", "ethiopian_birr", "Br", null);
        public static IotUnit euro => new IotUnit("Currency", "euro", "€", null);
        public static IotUnit falkland_islands_pound => new IotUnit("Currency", "falkland_islands_pound", "£", null);
        public static IotUnit fiji_dollar => new IotUnit("Currency", "fiji_dollar", "$", null);
        public static IotUnit gambian_dalasi => new IotUnit("Currency", "gambian_dalasi", "D", null);
        public static IotUnit georgian_lari => new IotUnit("Currency", "georgian_lari", "₾", null);
        public static IotUnit ghanaian_cedi => new IotUnit("Currency", "ghanaian_cedi", "₵", null);
        public static IotUnit gibraltar_pound => new IotUnit("Currency", "gibraltar_pound", "£", null);
        public static IotUnit guatemalan_quetzal => new IotUnit("Currency", "guatemalan_quetzal", "Q", null);
        public static IotUnit guinean_franc => new IotUnit("Currency", "guinean_franc", "FG", null);
        public static IotUnit guyanese_dollar => new IotUnit("Currency", "guyanese_dollar", "$", null);
        public static IotUnit haitian_gourde => new IotUnit("Currency", "haitian_gourde", "G", null);
        public static IotUnit honduran_lempira => new IotUnit("Currency", "honduran_lempira", "L", null);
        public static IotUnit hong_kong_dollar => new IotUnit("Currency", "hong_kong_dollar", "$", null);
        public static IotUnit hungarian_forint => new IotUnit("Currency", "hungarian_forint", "Ft", null);
        public static IotUnit icelandic_krona => new IotUnit("Currency", "icelandic_krona", "kr", null);
        public static IotUnit indian_rupee => new IotUnit("Currency", "indian_rupee", "₹", null);
        public static IotUnit indonesian_rupiah => new IotUnit("Currency", "indonesian_rupiah", "Rp", null);
        public static IotUnit iranian_rial => new IotUnit("Currency", "iranian_rial", "﷼", null);
        public static IotUnit iraqi_dinar => new IotUnit("Currency", "iraqi_dinar", "ع.د", null);
        public static IotUnit israeli_new_shekel => new IotUnit("Currency", "israeli_new_shekel", "₪", null);
        public static IotUnit jamaican_dollar => new IotUnit("Currency", "jamaican_dollar", "$", null);
        public static IotUnit japanese_yen => new IotUnit("Currency", "japanese_yen", "¥", null);
        public static IotUnit jordanian_dinar => new IotUnit("Currency", "jordanian_dinar", "د.ا", null);
        public static IotUnit kazakhstani_tenge => new IotUnit("Currency", "kazakhstani_tenge", "₸", null);
        public static IotUnit kenyan_shilling => new IotUnit("Currency", "kenyan_shilling", "KSh", null);
        public static IotUnit kuwaiti_dinar => new IotUnit("Currency", "kuwaiti_dinar", "د.ك", null);
        public static IotUnit kyrgyzstani_som => new IotUnit("Currency", "kyrgyzstani_som", "лв", null);
        public static IotUnit lao_kip => new IotUnit("Currency", "lao_kip", "₭", null);
        public static IotUnit lebanese_pound => new IotUnit("Currency", "lebanese_pound", "ل.ل", null);
        public static IotUnit lesotho_loti => new IotUnit("Currency", "lesotho_loti", "L", null);
        public static IotUnit liberian_dollar => new IotUnit("Currency", "liberian_dollar", "$", null);
        public static IotUnit libyan_dinar => new IotUnit("Currency", "libyan_dinar", "ل.د", null);
        public static IotUnit macanese_pataca => new IotUnit("Currency", "macanese_pataca", "MOP$", null);
        public static IotUnit malagasy_ariary => new IotUnit("Currency", "malagasy_ariary", "Ar", null);
        public static IotUnit malawian_kwacha => new IotUnit("Currency", "malawian_kwacha", "MK", null);
        public static IotUnit malaysian_ringgit => new IotUnit("Currency", "malaysian_ringgit", "RM", null);
        public static IotUnit maldivian_rufiyaa => new IotUnit("Currency", "maldivian_rufiyaa", "ރ.", null);
        public static IotUnit mauritanian_ouguiya => new IotUnit("Currency", "mauritanian_ouguiya", "UM", null);
        public static IotUnit mauritian_rupee => new IotUnit("Currency", "mauritian_rupee", "₨", null);
        public static IotUnit mexican_peso => new IotUnit("Currency", "mexican_peso", "$", null);
        public static IotUnit moldovan_leu => new IotUnit("Currency", "moldovan_leu", "L", null);
        public static IotUnit mongolian_togrog => new IotUnit("Currency", "mongolian_togrog", "₮", null);
        public static IotUnit moroccan_dirham => new IotUnit("Currency", "moroccan_dirham", "د.م.", null);
        public static IotUnit mozambican_metical => new IotUnit("Currency", "mozambican_metical", "MT", null);
        public static IotUnit myanmar_kyat => new IotUnit("Currency", "myanmar_kyat", "K", null);
        public static IotUnit namibian_dollar => new IotUnit("Currency", "namibian_dollar", "$", null);
        public static IotUnit nepalese_rupee => new IotUnit("Currency", "nepalese_rupee", "₨", null);
        public static IotUnit netherlands_antillean_guilder => new IotUnit("Currency", "netherlands_antillean_guilder", "ƒ", null);
        public static IotUnit new_taiwan_dollar => new IotUnit("Currency", "new_taiwan_dollar", "$", null);
        public static IotUnit new_zealand_dollar => new IotUnit("Currency", "new_zealand_dollar", "$", null);
        public static IotUnit nicaraguan_cordoba => new IotUnit("Currency", "nicaraguan_cordoba", "C$", null);
        public static IotUnit nigerian_naira => new IotUnit("Currency", "nigerian_naira", "₦", null);
        public static IotUnit north_korean_won => new IotUnit("Currency", "north_korean_won", "₩", null);
        public static IotUnit norwegian_krone => new IotUnit("Currency", "norwegian_krone", "kr", null);
        public static IotUnit omani_rial => new IotUnit("Currency", "omani_rial", "ر.ع.", null);
        public static IotUnit pakistani_rupee => new IotUnit("Currency", "pakistani_rupee", "₨", null);
        public static IotUnit panamanian_balboa => new IotUnit("Currency", "panamanian_balboa", "B/.", null);
        public static IotUnit papua_new_guinean_kina => new IotUnit("Currency", "papua_new_guinean_kina", "K", null);
        public static IotUnit paraguayan_guarani => new IotUnit("Currency", "paraguayan_guarani", "₲", null);
        public static IotUnit peruvian_sol => new IotUnit("Currency", "peruvian_sol", "S/", null);
        public static IotUnit philippine_peso => new IotUnit("Currency", "philippine_peso", "₱", null);
        public static IotUnit polish_zloty => new IotUnit("Currency", "polish_zloty", "zł", null);
        public static IotUnit qatar_riyal => new IotUnit("Currency", "qatar_riyal", "ر.ق", null);
        public static IotUnit romanian_leu => new IotUnit("Currency", "romanian_leu", "lei", null);
        public static IotUnit russian_ruble => new IotUnit("Currency", "russian_ruble", "₽", null);
        public static IotUnit rwandan_franc => new IotUnit("Currency", "rwandan_franc", "FRw", null);
        public static IotUnit saint_helena_pound => new IotUnit("Currency", "saint_helena_pound", "£", null);
        public static IotUnit samoan_tala => new IotUnit("Currency", "samoan_tala", "WS$", null);
        public static IotUnit saudi_riyal => new IotUnit("Currency", "saudi_riyal", "ر.س", null);
        public static IotUnit serbian_dinar => new IotUnit("Currency", "serbian_dinar", "дин.", null);
        public static IotUnit seychellois_rupee => new IotUnit("Currency", "seychellois_rupee", "₨", null);
        public static IotUnit sierra_leonean_leone => new IotUnit("Currency", "sierra_leonean_leone", "Le", null);
        public static IotUnit singapore_dollar => new IotUnit("Currency", "singapore_dollar", "$", null);
        public static IotUnit solomon_islands_dollar => new IotUnit("Currency", "solomon_islands_dollar", "$", null);
        public static IotUnit somali_shilling => new IotUnit("Currency", "somali_shilling", "Sh", null);
        public static IotUnit south_african_rand => new IotUnit("Currency", "south_african_rand", "R", null);
        public static IotUnit south_korean_won => new IotUnit("Currency", "south_korean_won", "₩", null);
        public static IotUnit south_sudanese_pound => new IotUnit("Currency", "south_sudanese_pound", "£", null);
        public static IotUnit sri_lankan_rupee => new IotUnit("Currency", "sri_lankan_rupee", "₨", null);
        public static IotUnit sudanese_pound => new IotUnit("Currency", "sudanese_pound", "ج.س.", null);
        public static IotUnit surinamese_dollar => new IotUnit("Currency", "surinamese_dollar", "$", null);
        public static IotUnit swazi_lilangeni => new IotUnit("Currency", "swazi_lilangeni", "L", null);
        public static IotUnit swedish_krona => new IotUnit("Currency", "swedish_krona", "kr", null);
        public static IotUnit swiss_franc => new IotUnit("Currency", "swiss_franc", "Fr", null);
        public static IotUnit syrian_pound => new IotUnit("Currency", "syrian_pound", "£", null);
        public static IotUnit taiwanese_dollar => new IotUnit("Currency", "taiwanese_dollar", "NT$", null);
        public static IotUnit tajikistani_somoni => new IotUnit("Currency", "tajikistani_somoni", "ЅМ", null);
        public static IotUnit tanzanian_shilling => new IotUnit("Currency", "tanzanian_shilling", "Sh", null);
        public static IotUnit thai_baht => new IotUnit("Currency", "thai_baht", "฿", null);
        public static IotUnit tonga_paanga => new IotUnit("Currency", "tonga_paanga", "T$", null);
        public static IotUnit trinidad_and_tobago_dollar => new IotUnit("Currency", "trinidad_and_tobago_dollar", "$", null);
        public static IotUnit tunisian_dinar => new IotUnit("Currency", "tunisian_dinar", "د.ت", null);
        public static IotUnit turkish_lira => new IotUnit("Currency", "turkish_lira", "₺", null);
        public static IotUnit turkmenistani_manat => new IotUnit("Currency", "turkmenistani_manat", "m", null);
        public static IotUnit ugandan_shilling => new IotUnit("Currency", "ugandan_shilling", "USh", null);
        public static IotUnit ukrainian_hryvnia => new IotUnit("Currency", "ukrainian_hryvnia", "₴", null);
        public static IotUnit united_arab_emirates_dirham => new IotUnit("Currency", "united_arab_emirates_dirham", "د.إ", null);
        public static IotUnit uruguayan_peso => new IotUnit("Currency", "uruguayan_peso", "$U", null);
        public static IotUnit uzbekistani_som => new IotUnit("Currency", "uzbekistani_som", "so'm", null);
        public static IotUnit vanuatu_vatu => new IotUnit("Currency", "vanuatu_vatu", "VT", null);
        public static IotUnit venezuelan_bolivar => new IotUnit("Currency", "venezuelan_bolivar", "Bs.", null);
        public static IotUnit vietnamese_dong => new IotUnit("Currency", "vietnamese_dong", "₫", null);
        public static IotUnit yemeni_rial => new IotUnit("Currency", "yemeni_rial", "﷼", null);
        public static IotUnit zambian_kwacha => new IotUnit("Currency", "zambian_kwacha", "ZK", null);
        public static IotUnit zimbabwean_dollar => new IotUnit("Currency", "zimbabwean_dollar", "$", null);

        // DataRate
        public static IotUnit bits_per_second => new IotUnit("DataRate", "bits_per_second", "bps",
            new Dictionary<string, string>
            {
        { "kilobits_per_second", "value / 1000" }, // Convert kbps to bps (target: bits_per_second)
        { "megabits_per_second", "value / 1e6" }, // Convert Mbps to bps (target: bits_per_second)
        { "gigabits_per_second", "value / 1e9" } // Convert Gbps to bps (target: bits_per_second)
            }
        );

        public static IotUnit kilobits_per_second => new IotUnit("DataRate", "kilobits_per_second", "kbps",
            new Dictionary<string, string>
            {
                { "bits_per_second", "value * 1000" }, // Convert bps to kbps (target: kilobits_per_second)
                { "megabits_per_second", "value / 1000" }, // Convert Mbps to kbps (target: kilobits_per_second)
                { "gigabits_per_second", "value / 1e6" } // Convert Gbps to kbps (target: kilobits_per_second)
            }
        );

        public static IotUnit megabits_per_second => new IotUnit("DataRate", "megabits_per_second", "Mbps",
            new Dictionary<string, string>
            {
                { "bits_per_second", "value * 1e6" }, // Convert bps to Mbps (target: megabits_per_second)
                { "kilobits_per_second", "value * 1000" }, // Convert kbps to Mbps (target: megabits_per_second)
                { "gigabits_per_second", "value / 1000" } // Convert Gbps to Mbps (target: megabits_per_second)
            }
        );

        public static IotUnit gigabits_per_second => new IotUnit("DataRate", "gigabits_per_second", "Gbps",
            new Dictionary<string, string>
            {
                { "bits_per_second", "value * 1e9" }, // Convert bps to Gbps (target: gigabits_per_second)
                { "kilobits_per_second", "value * 1e6" }, // Convert kbps to Gbps (target: gigabits_per_second)
                { "megabits_per_second", "value * 1000" } // Convert Mbps to Gbps (target: gigabits_per_second)
            }
        );


        // DataStorage
        public static IotUnit bytes => new IotUnit("DataStorage", "bytes", "B",
            new Dictionary<string, string>
            {
                { "kilobytes", "value / 1024" }, // Convert KB to B (target: bytes)
                { "megabytes", "value / (1024 * 1024)" }, // Convert MB to B (target: bytes)
                { "gigabytes", "value / (1024 * 1024 * 1024)" }, // Convert GB to B (target: bytes)
                { "terabytes", "value / (1024 * 1024 * 1024 * 1024)" }, // Convert TB to B (target: bytes)
                { "petabytes", "value / (1024L * 1024 * 1024 * 1024 * 1024)" }, // Convert PB to B (target: bytes)
                { "exabytes", "value / (1024L * 1024 * 1024 * 1024 * 1024 * 1024)" }, // Convert EB to B (target: bytes)
                { "zettabytes", "value / (1024L * 1024 * 1024 * 1024 * 1024 * 1024 * 1024)" }, // Convert ZB to B (target: bytes)
                { "yottabytes", "value / (1024L * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024)" } // Convert YB to B (target: bytes)
            }
        );

        public static IotUnit kilobytes => new IotUnit("DataStorage", "kilobytes", "KB",
            new Dictionary<string, string>
            {
                { "bytes", "value * 1024" }, // Convert B to KB (target: kilobytes)
                { "megabytes", "value / 1024" }, // Convert MB to KB (target: kilobytes)
                { "gigabytes", "value / (1024 * 1024)" }, // Convert GB to KB (target: kilobytes)
                { "terabytes", "value / (1024 * 1024 * 1024)" }, // Convert TB to KB (target: kilobytes)
                { "petabytes", "value / (1024L * 1024 * 1024 * 1024)" }, // Convert PB to KB (target: kilobytes)
                { "exabytes", "value / (1024L * 1024 * 1024 * 1024 * 1024)" }, // Convert EB to KB (target: kilobytes)
                { "zettabytes", "value / (1024L * 1024 * 1024 * 1024 * 1024 * 1024)" }, // Convert ZB to KB (target: kilobytes)
                { "yottabytes", "value / (1024L * 1024 * 1024 * 1024 * 1024 * 1024 * 1024)" } // Convert YB to KB (target: kilobytes)
            }
        );

        public static IotUnit megabytes => new IotUnit("DataStorage", "megabytes", "MB",
            new Dictionary<string, string>
            {
                { "bytes", "value * (1024 * 1024)" }, // Convert B to MB (target: megabytes)
                { "kilobytes", "value * 1024" }, // Convert KB to MB (target: megabytes)
                { "gigabytes", "value / 1024" }, // Convert GB to MB (target: megabytes)
                { "terabytes", "value / (1024 * 1024)" }, // Convert TB to MB (target: megabytes)
                { "petabytes", "value / (1024L * 1024 * 1024)" }, // Convert PB to MB (target: megabytes)
                { "exabytes", "value / (1024L * 1024 * 1024 * 1024)" }, // Convert EB to MB (target: megabytes)
                { "zettabytes", "value / (1024L * 1024 * 1024 * 1024 * 1024)" }, // Convert ZB to MB (target: megabytes)
                { "yottabytes", "value / (1024L * 1024 * 1024 * 1024 * 1024 * 1024)" } // Convert YB to MB (target: megabytes)
            }
        );

        public static IotUnit gigabytes => new IotUnit("DataStorage", "gigabytes", "GB",
            new Dictionary<string, string>
            {
                { "bytes", "value * (1024 * 1024 * 1024)" }, // Convert B to GB (target: gigabytes)
                { "kilobytes", "value * (1024 * 1024)" }, // Convert KB to GB (target: gigabytes)
                { "megabytes", "value * 1024" }, // Convert MB to GB (target: gigabytes)
                { "terabytes", "value / 1024" }, // Convert TB to GB (target: gigabytes)
                { "petabytes", "value / (1024L * 1024)" }, // Convert PB to GB (target: gigabytes)
                { "exabytes", "value / (1024L * 1024 * 1024)" }, // Convert EB to GB (target: gigabytes)
                { "zettabytes", "value / (1024L * 1024 * 1024 * 1024)" }, // Convert ZB to GB (target: gigabytes)
                { "yottabytes", "value / (1024L * 1024 * 1024 * 1024 * 1024)" } // Convert YB to GB (target: gigabytes)
            }
        );

        public static IotUnit terabytes => new IotUnit("DataStorage", "terabytes", "TB",
            new Dictionary<string, string>
            {
            { "bytes", "value * (1024L * 1024 * 1024 * 1024)" }, // Convert B to TB (target: terabytes)
            { "kilobytes", "value * (1024 * 1024 * 1024)" }, // Convert KB to TB (target: terabytes)
            { "megabytes", "value * (1024 * 1024)" }, // Convert MB to TB (target: terabytes)
            { "gigabytes", "value * 1024" }, // Convert GB to TB (target: terabytes)
            { "petabytes", "value / 1024" }, // Convert PB to TB (target: terabytes)
            { "exabytes", "value / (1024L * 1024)" }, // Convert EB to TB (target: terabytes)
            { "zettabytes", "value / (1024L * 1024 * 1024)" }, // Convert ZB to TB (target: terabytes)
            { "yottabytes", "value / (1024L * 1024 * 1024 * 1024)" } // Convert YB to TB (target: terabytes)
            }
        );

        public static IotUnit petabytes => new IotUnit("DataStorage", "petabytes", "PB",
            new Dictionary<string, string>
            {
                { "bytes", "value * (1024L * 1024 * 1024 * 1024 * 1024)" }, // Convert B to PB (target: petabytes)
                { "kilobytes", "value * (1024L * 1024 * 1024 * 1024)" }, // Convert KB to PB (target: petabytes)
                { "megabytes", "value * (1024L * 1024 * 1024)" }, // Convert MB to PB (target: petabytes)
                { "gigabytes", "value * (1024L * 1024)" }, // Convert GB to PB (target: petabytes)
                { "terabytes", "value * 1024" }, // Convert TB to PB (target: petabytes)
                { "exabytes", "value / 1024" }, // Convert EB to PB (target: petabytes)
                { "zettabytes", "value / (1024L * 1024)" }, // Convert ZB to PB (target: petabytes)
                { "yottabytes", "value / (1024L * 1024 * 1024)" } // Convert YB to PB (target: petabytes)
            }
        );

        public static IotUnit exabytes => new IotUnit("DataStorage", "exabytes", "EB",
            new Dictionary<string, string>
            {
                { "bytes", "value * (1024L * 1024 * 1024 * 1024 * 1024 * 1024)" }, // Convert B to EB (target: exabytes)
                { "kilobytes", "value * (1024L * 1024 * 1024 * 1024 * 1024)" }, // Convert KB to EB (target: exabytes)
                { "megabytes", "value * (1024L * 1024 * 1024 * 1024)" }, // Convert MB to EB (target: exabytes)
                { "gigabytes", "value * (1024L * 1024 * 1024)" }, // Convert GB to EB (target: exabytes)
                { "terabytes", "value * (1024L * 1024)" }, // Convert TB to EB (target: exabytes)
                { "petabytes", "value * 1024" }, // Convert PB to EB (target: exabytes)
                { "zettabytes", "value / 1024" }, // Convert ZB to EB (target: exabytes)
                { "yottabytes", "value / (1024L * 1024)" } // Convert YB to EB (target: exabytes)
            }
        );

        public static IotUnit zettabytes => new IotUnit("DataStorage", "zettabytes", "ZB",
            new Dictionary<string, string>
            {
                { "bytes", "value * (1024L * 1024 * 1024 * 1024 * 1024 * 1024 * 1024)" }, // Convert B to ZB (target: zettabytes)
                { "kilobytes", "value * (1024L * 1024 * 1024 * 1024 * 1024 * 1024)" }, // Convert KB to ZB (target: zettabytes)
                { "megabytes", "value * (1024L * 1024 * 1024 * 1024 * 1024)" }, // Convert MB to ZB (target: zettabytes)
                { "gigabytes", "value * (1024L * 1024 * 1024 * 1024)" }, // Convert GB to ZB (target: zettabytes)
                { "terabytes", "value * (1024L * 1024 * 1024)" }, // Convert TB to ZB (target: zettabytes)
                { "petabytes", "value * (1024L * 1024)" }, // Convert PB to ZB (target: zettabytes)
                { "exabytes", "value * 1024" }, // Convert EB to ZB (target: zettabytes)
                { "yottabytes", "value / 1024" } // Convert YB to ZB (target: zettabytes)
            }
        );

        public static IotUnit yottabytes => new IotUnit("DataStorage", "yottabytes", "YB",
            new Dictionary<string, string>
            {
                { "bytes", "value * (1024L * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024)" }, // Convert B to YB (target: yottabytes)
                { "kilobytes", "value * (1024L * 1024 * 1024 * 1024 * 1024 * 1024 * 1024)" }, // Convert KB to YB (target: yottabytes)
                { "megabytes", "value * (1024L * 1024 * 1024 * 1024 * 1024 * 1024)" }, // Convert MB to YB (target: yottabytes)
                { "gigabytes", "value * (1024L * 1024 * 1024 * 1024 * 1024)" }, // Convert GB to YB (target: yottabytes)
                { "terabytes", "value * (1024L * 1024 * 1024 * 1024)" }, // Convert TB to YB (target: yottabytes)
                { "petabytes", "value * (1024L * 1024 * 1024)" }, // Convert PB to YB (target: yottabytes)
                { "exabytes", "value * (1024L * 1024)" }, // Convert EB to YB (target: yottabytes)
                { "zettabytes", "value * 1024" } // Convert ZB to YB (target: yottabytes)
            }
        );

        // ElectricCharge
        public static IotUnit ampere_hours => new IotUnit("ElectricCharge", "ampere_hours", "Ah",
            new Dictionary<string, string>
            {
                { "coulombs", "value * 3600" } // Convert C to Ah (target: ampere_hours)
            }
        );

        public static IotUnit coulombs => new IotUnit("ElectricCharge", "coulombs", "C",
            new Dictionary<string, string>
            {
                { "ampere_hours", "value / 3600" } // Convert Ah to C (target: coulombs)
            }
        );


        // ElectricPotential
        public static IotUnit kilovolts => new IotUnit("ElectricPotential", "kilovolts", "kV",
            new Dictionary<string, string>
            {
                { "volts", "value * 1000" }, // Convert V to kV (target: kilovolts)
                { "millivolts", "value * 1e6" } // Convert mV to kV (target: kilovolts)
            }
        );

        public static IotUnit millivolts => new IotUnit("ElectricPotential", "millivolts", "mV",
            new Dictionary<string, string>
            {
                { "volts", "value / 1000" }, // Convert V to mV (target: millivolts)
                { "kilovolts", "value / 1e6" } // Convert kV to mV (target: millivolts)
            }
        );

        public static IotUnit volts => new IotUnit("ElectricPotential", "volts", "V",
            new Dictionary<string, string>
            {
                { "kilovolts", "value / 1000" }, // Convert kV to V (target: volts)
                { "millivolts", "value * 1000" } // Convert mV to V (target: volts)
            }
        );


        // ElectricResistance
        public static IotUnit kilohms => new IotUnit("ElectricResistance", "kilohms", "kΩ",
            new Dictionary<string, string>
            {
                { "ohms", "value * 1000" }, // Convert Ω to kΩ (target: kilohms)
                { "megohms", "value / 1000" }, // Convert MΩ to kΩ (target: kilohms)
                { "milliohms", "value * 1e6" } // Convert mΩ to kΩ (target: kilohms)
            }
        );

        public static IotUnit megohms => new IotUnit("ElectricResistance", "megohms", "MΩ",
            new Dictionary<string, string>
            {
                { "ohms", "value * 1e6" }, // Convert Ω to MΩ (target: megohms)
                { "kilohms", "value * 1000" }, // Convert kΩ to MΩ (target: megohms)
                { "milliohms", "value * 1e9" } // Convert mΩ to MΩ (target: megohms)
            }
        );

        public static IotUnit milliohms => new IotUnit("ElectricResistance", "milliohms", "mΩ",
            new Dictionary<string, string>
            {
                { "ohms", "value / 1000" }, // Convert Ω to mΩ (target: milliohms)
                { "kilohms", "value / 1e6" }, // Convert kΩ to mΩ (target: milliohms)
                { "megohms", "value / 1e9" } // Convert MΩ to mΩ (target: milliohms)
            }
        );

        public static IotUnit ohms => new IotUnit("ElectricResistance", "ohms", "Ω",
            new Dictionary<string, string>
            {
                { "kilohms", "value / 1000" }, // Convert kΩ to Ω (target: ohms)
                { "megohms", "value / 1e6" }, // Convert MΩ to Ω (target: ohms)
                { "milliohms", "value * 1000" } // Convert mΩ to Ω (target: ohms)
            }
        );


        // Electrical
        public static IotUnit amperes => new IotUnit("Electrical", "amperes", "A",
            new Dictionary<string, string>
            {
                { "microamperes", "value * 1e6" }, // Convert μA to A (target: amperes)
                { "milliamperes", "value * 1000" } // Convert mA to A (target: amperes)
            }
        );

        public static IotUnit microamperes => new IotUnit("Electrical", "microamperes", "μA",
            new Dictionary<string, string>
            {
                { "amperes", "value / 1e6" }, // Convert A to μA (target: microamperes)
                { "milliamperes", "value / 1000" } // Convert mA to μA (target: microamperes)
            }
        );

        public static IotUnit milliamperes => new IotUnit("Electrical", "milliamperes", "mA",
            new Dictionary<string, string>
            {
                { "amperes", "value / 1000" }, // Convert A to mA (target: milliamperes)
                { "microamperes", "value * 1000" } // Convert μA to mA (target: milliamperes)
            }
        );

        public static IotUnit henrys => new IotUnit("Electrical", "henrys", "H",
            new Dictionary<string, string>
            {
                { "millihenrys", "value * 1000" }, // Convert mH to H (target: henrys)
                { "microhenrys", "value * 1e6" }, // Convert μH to H (target: henrys)
                { "nanohenrys", "value * 1e9" } // Convert nH to H (target: henrys)
            }
        );

        public static IotUnit millihenrys => new IotUnit("Electrical", "millihenrys", "mH",
            new Dictionary<string, string>
            {
                { "henrys", "value / 1000" }, // Convert H to mH (target: millihenrys)
                { "microhenrys", "value * 1000" }, // Convert μH to mH (target: millihenrys)
                { "nanohenrys", "value * 1e6" } // Convert nH to mH (target: millihenrys)
            }
        );

        public static IotUnit microhenrys => new IotUnit("Electrical", "microhenrys", "μH",
            new Dictionary<string, string>
            {
                { "henrys", "value / 1e6" }, // Convert H to μH (target: microhenrys)
                { "millihenrys", "value / 1000" }, // Convert mH to μH (target: microhenrys)
                { "nanohenrys", "value * 1000" } // Convert nH to μH (target: microhenrys)
            }
        );

        public static IotUnit nanohenrys => new IotUnit("Electrical", "nanohenrys", "nH",
            new Dictionary<string, string>
            {
                { "henrys", "value / 1e9" }, // Convert H to nH (target: nanohenrys)
                { "millihenrys", "value / 1e6" }, // Convert mH to nH (target: nanohenrys)
                { "microhenrys", "value / 1000" } // Convert μH to nH (target: nanohenrys)
            }
        );

        public static IotUnit watts => new IotUnit("Electrical", "watts", "W",
            new Dictionary<string, string>
            {
                { "watt_hours", "value / 3600" }, // Convert Wh to W (target: watts)
                { "watt_seconds", "value * 1" } // Convert Ws to W (target: watts)
            }
        );

        public static IotUnit watt_seconds => new IotUnit("Electrical", "watt_seconds", "Ws",
            new Dictionary<string, string>
            {
                { "watts", "value * 1" }, // Convert W to Ws (target: watt_seconds)
                { "watt_hours", "value / 3600" } // Convert Wh to Ws (target: watt_seconds)
            }
        );

        public static IotUnit watt_hours => new IotUnit("Electrical", "watt_hours", "Wh",
            new Dictionary<string, string>
            {
                { "watts", "value * 3600" }, // Convert W to Wh (target: watt_hours)
                { "watt_seconds", "value * 3600" } // Convert Ws to Wh (target: watt_hours)
            }
        );

        public static IotUnit amperes_per_meter => new IotUnit("Electrical", "amperes_per_meter", "A/m",
            new Dictionary<string, string>
            {
                // Add conversion rules if applicable
            }
        );

        public static IotUnit amperes_per_square_meter => new IotUnit("Electrical", "amperes_per_square_meter", "A/m²",
            new Dictionary<string, string>
            {
                // Add conversion rules if applicable
            }
        );

        public static IotUnit ampere_square_meters => new IotUnit("Electrical", "ampere_square_meters", "A·m²",
            new Dictionary<string, string>
            {
                // Add conversion rules if applicable
            }
        );

        public static IotUnit bars => new IotUnit("Electrical", "bars", "bar",
            new Dictionary<string, string>
            {
                // Add conversion rules if applicable
            }
        );

        public static IotUnit decibels => new IotUnit("Electrical", "decibels", "dB",
            new Dictionary<string, string>
            {
                // Add conversion rules if applicable
            }
        );

        public static IotUnit decibels_millivolt => new IotUnit("Electrical", "decibels_millivolt", "dBmV",
            new Dictionary<string, string>
            {
                // Add conversion rules if applicable
            }
        );

        public static IotUnit decibels_volt => new IotUnit("Electrical", "decibels_volt", "dBV",
            new Dictionary<string, string>
            {
                // Add conversion rules if applicable
            }
        );

        public static IotUnit degrees_phase => new IotUnit("Electrical", "degrees_phase", "°",
            new Dictionary<string, string>
            {
                // Add conversion rules if applicable
            }
        );

        public static IotUnit kilovolt_amperes => new IotUnit("Electrical", "kilovolt_amperes", "kVA",
            new Dictionary<string, string>
            {
                { "megavolt_amperes", "value / 1000" }, // Convert MVA to kVA (target: kilovolt_amperes)
                { "millivolt_amperes", "value * 1e6" } // Convert mVA to kVA (target: kilovolt_amperes)
            }
        );

        public static IotUnit kilovolt_amperes_reactive => new IotUnit("Electrical", "kilovolt_amperes_reactive", "kVAR",
            new Dictionary<string, string>
            {
                { "megavolt_amperes_reactive", "value / 1000" } // Convert MVAR to kVAR (target: kilovolt_amperes_reactive)
            }
        );

        public static IotUnit megavolt_amperes => new IotUnit("Electrical", "megavolt_amperes", "MVA",
            new Dictionary<string, string>
            {
                { "kilovolt_amperes", "value * 1000" }, // Convert kVA to MVA (target: megavolt_amperes)
                { "millivolt_amperes", "value * 1e9" } // Convert mVA to MVA (target: megavolt_amperes)
            }
        );

        public static IotUnit megavolt_amperes_reactive => new IotUnit("Electrical", "megavolt_amperes_reactive", "MVAR",
            new Dictionary<string, string>
            {
                { "kilovolt_amperes_reactive", "value * 1000" } // Convert kVAR to MVAR (target: megavolt_amperes_reactive)
            }
        );

        public static IotUnit millivolt_amperes => new IotUnit("Electrical", "millivolt_amperes", "mVA",
            new Dictionary<string, string>
            {
                { "kilovolt_amperes", "value / 1e6" }, // Convert kVA to mVA (target: millivolt_amperes)
                { "megavolt_amperes", "value / 1e9" } // Convert MVA to mVA (target: millivolt_amperes)
            }
        );

        public static IotUnit siemens => new IotUnit("Electrical", "siemens", "S",
            new Dictionary<string, string>
            {
                // Add conversion rules if applicable
            }
        );

        public static IotUnit teslas => new IotUnit("Electrical", "teslas", "T",
            new Dictionary<string, string>
            {
                // Add conversion rules if applicable
            }
        );

        public static IotUnit volts_dc => new IotUnit("Electrical", "volts_dc", "VDC",
            new Dictionary<string, string>
            {
                // Add conversion rules if applicable
            }
        );

        public static IotUnit watts_per_meter_kelvin => new IotUnit("Electrical", "watts_per_meter_kelvin", "W/mK",
            new Dictionary<string, string>
            {
                // Add conversion rules if applicable
            }
        );

        public static IotUnit watt_per_square_meter => new IotUnit("Electrical", "watt_per_square_meter", "W/m²",
            new Dictionary<string, string>
            {
                // Add conversion rules if applicable
            }
        );


        // Energy Units
        public static IotUnit ampere_seconds => new IotUnit("Energy", "ampere_seconds", "As",
            new Dictionary<string, string>
            {
                { "joules", "value * 1" } // Convert J to As (target: ampere_seconds)
            }
        );

        public static IotUnit joules => new IotUnit("Energy", "joules", "J",
            new Dictionary<string, string>
            {
                { "ampere_seconds", "value * 1" }, // Convert As to J (target: joules)
                { "kilojoules", "value / 1000" }, // Convert kJ to J (target: joules)
                { "megajoules", "value / 1e6" } // Convert MJ to J (target: joules)
            }
        );

        public static IotUnit kilojoules => new IotUnit("Energy", "kilojoules", "kJ",
            new Dictionary<string, string>
            {
                { "joules", "value * 1000" }, // Convert J to kJ (target: kilojoules)
                { "megajoules", "value / 1000" } // Convert MJ to kJ (target: kilojoules)
            }
        );

        public static IotUnit megajoules => new IotUnit("Energy", "megajoules", "MJ",
            new Dictionary<string, string>
            {
                { "joules", "value * 1e6" }, // Convert J to MJ (target: megajoules)
                { "kilojoules", "value * 1000" } // Convert kJ to MJ (target: megajoules)
            }
        );

        public static IotUnit watt_hours_energy => new IotUnit("Energy", "watt_hours", "Wh",
            new Dictionary<string, string>
            {
                { "kilowatt_hours", "value / 1000" }, // Convert kWh to Wh (target: watt_hours_energy)
                { "megawatt_hours", "value / 1e6" } // Convert MWh to Wh (target: watt_hours_energy)
            }
        );

        public static IotUnit kilowatt_hours => new IotUnit("Energy", "kilowatt_hours", "kWh",
            new Dictionary<string, string>
            {
                { "watt_hours_energy", "value * 1000" }, // Convert Wh to kWh (target: kilowatt_hours)
                { "megawatt_hours", "value / 1000" } // Convert MWh to kWh (target: kilowatt_hours)
            }
        );

        public static IotUnit megawatt_hours => new IotUnit("Energy", "megawatt_hours", "MWh",
            new Dictionary<string, string>
            {
                { "watt_hours_energy", "value * 1e6" }, // Convert Wh to MWh (target: megawatt_hours)
                { "kilowatt_hours", "value * 1000" } // Convert kWh to MWh (target: megawatt_hours)
            }
        );

        public static IotUnit btus => new IotUnit("Energy", "btus", "BTU",
            new Dictionary<string, string>
            {
                { "kilo_btus", "value / 1000" } // Convert kBTU to BTU (target: btus)
            }
        );

        public static IotUnit kilo_btus => new IotUnit("Energy", "kilo_btus", "kBTU",
            new Dictionary<string, string>
            {
                { "btus", "value * 1000" } // Convert BTU to kBTU (target: kilo_btus)
            }
        );

        public static IotUnit ton_hours => new IotUnit("Energy", "ton_hours", "ton-h",
            new Dictionary<string, string>
            {
                // Add conversion rules if applicable
            }
        );

        public static IotUnit kilovolt_ampere_hours => new IotUnit("Energy", "kilovolt_ampere_hours", "kVAh",
            new Dictionary<string, string>
            {
                { "megavolt_ampere_hours", "value / 1000" } // Convert MVAh to kVAh (target: kilovolt_ampere_hours)
            }
        );

        public static IotUnit megavolt_ampere_hours => new IotUnit("Energy", "megavolt_ampere_hours", "MVAh",
            new Dictionary<string, string>
            {
                { "kilovolt_ampere_hours", "value * 1000" } // Convert kVAh to MVAh (target: megavolt_ampere_hours)
            }
        );

        public static IotUnit volt_ampere_hours => new IotUnit("Energy", "volt_ampere_hours", "VAh",
            new Dictionary<string, string>
            {
                { "kilovolt_ampere_hours", "value / 1000" } // Convert kVAh to VAh (target: volt_ampere_hours)
            }
        );

        public static IotUnit volt_square_hours => new IotUnit("Energy", "volt_square_hours", "V²h",
            new Dictionary<string, string>
            {
                // Add conversion rules if applicable
            }
        );

        public static IotUnit kilojoules_per_kilogram => new IotUnit("Energy", "kilojoules_per_kilogram", "kJ/kg",
            new Dictionary<string, string>
            {
                // Add conversion rules if applicable
            }
        );

        public static IotUnit kilovolt_ampere_hours_reactive => new IotUnit("Energy", "kilovolt_ampere_hours_reactive", "kVARh",
            new Dictionary<string, string>
            {
                { "megavolt_ampere_hours_reactive", "value / 1000" } // Convert MVARh to kVARh (target: kilovolt_ampere_hours_reactive)
            }
        );

        public static IotUnit megavolt_ampere_hours_reactive => new IotUnit("Energy", "megavolt_ampere_hours_reactive", "MVARh",
            new Dictionary<string, string>
            {
                { "kilovolt_ampere_hours_reactive", "value * 1000" } // Convert kVARh to MVARh (target: megavolt_ampere_hours_reactive)
            }
        );

        public static IotUnit kilowatt_hours_reactive => new IotUnit("Energy", "kilowatt_hours_reactive", "kWh",
            new Dictionary<string, string>
            {
                { "megawatt_hours_reactive", "value / 1000" } // Convert MWh to kWh (target: kilowatt_hours_reactive)
            }
        );

        public static IotUnit megawatt_hours_reactive => new IotUnit("Energy", "megawatt_hours_reactive", "MWh",
            new Dictionary<string, string>
            {
                { "kilowatt_hours_reactive", "value * 1000" } // Convert kWh to MWh (target: megawatt_hours_reactive)
            }
        );

        public static IotUnit volt_ampere_hours_reactive => new IotUnit("Energy", "volt_ampere_hours_reactive", "VARh",
            new Dictionary<string, string>
            {
                { "kilovolt_ampere_hours_reactive", "value / 1000" } // Convert kVARh to VARh (target: volt_ampere_hours_reactive)
            }
        );

        public static IotUnit watt_hours_reactive => new IotUnit("Energy", "watt_hours_reactive", "Wh",
            new Dictionary<string, string>
            {
                { "kilowatt_hours_reactive", "value / 1000" }, // Convert kWh to Wh (target: watt_hours_reactive)
                { "megawatt_hours_reactive", "value / 1e6" } // Convert MWh to Wh (target: watt_hours_reactive)
            }
        );





        // Energy Density Units
        public static IotUnit joules_per_cubic_meter => new IotUnit("EnergyDensity", "joules_per_cubic_meter", "J/m³",
            new Dictionary<string, string>
            {
                { "watt_hours_per_cubic_meter", "value / 3600" }, // Convert Wh/m³ to J/m³
                { "kilowatt_hours_per_square_meter", "value / (3600 * 1000)" }, // Convert kWh/m² to J/m³
                { "megajoules_per_square_meter", "value / 1e6" }, // Convert MJ/m² to J/m³
                { "kilowatt_hours_per_square_foot", "value / (3600 * 10.764)" }, // Convert kWh/ft² to J/m³
                { "megajoules_per_square_foot", "value / (1e6 * 10.764)" } // Convert MJ/ft² to J/m³
            }
        );

        public static IotUnit watt_hours_per_cubic_meter => new IotUnit("EnergyDensity", "watt_hours_per_cubic_meter", "Wh/m³",
            new Dictionary<string, string>
            {
                { "joules_per_cubic_meter", "value * 3600" }, // Convert J/m³ to Wh/m³
                { "kilowatt_hours_per_square_meter", "value / 1000" }, // Convert kWh/m² to Wh/m³
                { "megajoules_per_square_meter", "value / (3.6 * 1000)" }, // Convert MJ/m² to Wh/m³
                { "kilowatt_hours_per_square_foot", "value / 10.764" }, // Convert kWh/ft² to Wh/m³
                { "megajoules_per_square_foot", "value / (3.6 * 10.764)" } // Convert MJ/ft² to Wh/m³
            }
        );

        public static IotUnit kilowatt_hours_per_square_meter => new IotUnit("EnergyDensity", "kilowatt_hours_per_square_meter", "kWh/m²",
            new Dictionary<string, string>
            {
                { "joules_per_cubic_meter", "value * (3600 * 1000)" }, // Convert J/m³ to kWh/m²
                { "watt_hours_per_cubic_meter", "value * 1000" }, // Convert Wh/m³ to kWh/m²
                { "megajoules_per_square_meter", "value * 3.6" }, // Convert MJ/m² to kWh/m²
                { "kilowatt_hours_per_square_foot", "value / 10.764" }, // Convert kWh/ft² to kWh/m²
                { "megajoules_per_square_foot", "value / 2.99" } // Convert MJ/ft² to kWh/m²
            }
        );

        public static IotUnit megajoules_per_square_meter => new IotUnit("EnergyDensity", "megajoules_per_square_meter", "MJ/m²",
            new Dictionary<string, string>
            {
                { "joules_per_cubic_meter", "value * 1e6" }, // Convert J/m³ to MJ/m²
                { "watt_hours_per_cubic_meter", "value * (3.6 * 1000)" }, // Convert Wh/m³ to MJ/m²
                { "kilowatt_hours_per_square_meter", "value / 3.6" }, // Convert kWh/m² to MJ/m²
                { "kilowatt_hours_per_square_foot", "value / 38.8" }, // Convert kWh/ft² to MJ/m²
                { "megajoules_per_square_foot", "value / 10.764" } // Convert MJ/ft² to MJ/m²
            }
        );

        public static IotUnit kilowatt_hours_per_square_foot => new IotUnit("EnergyDensity", "kilowatt_hours_per_square_foot", "kWh/ft²",
            new Dictionary<string, string>
            {
                { "joules_per_cubic_meter", "value * (3600 * 10.764)" }, // Convert J/m³ to kWh/ft²
                { "watt_hours_per_cubic_meter", "value * 10.764" }, // Convert Wh/m³ to kWh/ft²
                { "kilowatt_hours_per_square_meter", "value * 10.764" }, // Convert kWh/m² to kWh/ft²
                { "megajoules_per_square_meter", "value * 38.8" }, // Convert MJ/m² to kWh/ft²
                { "megajoules_per_square_foot", "value * 3.6" } // Convert MJ/ft² to kWh/ft²
            }
        );

        public static IotUnit megajoules_per_square_foot => new IotUnit("EnergyDensity", "megajoules_per_square_foot", "MJ/ft²",
            new Dictionary<string, string>
            {
                { "joules_per_cubic_meter", "value * (1e6 * 10.764)" }, // Convert J/m³ to MJ/ft²
                { "watt_hours_per_cubic_meter", "value * (3.6 * 10.764)" }, // Convert Wh/m³ to MJ/ft²
                { "kilowatt_hours_per_square_meter", "value * 2.99" }, // Convert kWh/m² to MJ/ft²
                { "megajoules_per_square_meter", "value * 10.764" }, // Convert MJ/m² to MJ/ft²
                { "kilowatt_hours_per_square_foot", "value / 3.6" } // Convert kWh/ft² to MJ/ft²
            }
        );


        // Energy Specific Units
        public static IotUnit joule_seconds => new IotUnit("EnergySpecific", "joule_seconds", "Js", null);

        // Enthalpy Units
        public static IotUnit btus_per_pound => new IotUnit("Enthalpy", "btus_per_pound", "BTU/lb",
            new Dictionary<string, string>
            {
                { "btus_per_pound_dry_air", "value * 1" }, // Convert BTU/lb to BTU/lb (identical)
                { "joules_per_degree_kelvin", "value * 4184" }, // Convert J/K to BTU/lb
                { "joules_per_kilogram_dry_air", "value * 2326" }, // Convert J/kg to BTU/lb
                { "joules_per_kilogram_degree_kelvin", "value * 4184" }, // Convert J/(kg·K) to BTU/lb
                { "kilojoules_per_degree_kelvin", "value * 4.184" } // Convert kJ/K to BTU/lb
            }
        );

        public static IotUnit btus_per_pound_dry_air => new IotUnit("Enthalpy", "btus_per_pound_dry_air", "BTU/lb",
            new Dictionary<string, string>
            {
                { "btus_per_pound", "value * 1" }, // Convert BTU/lb to BTU/lb (identical)
                { "joules_per_degree_kelvin", "value * 4184" }, // Convert J/K to BTU/lb
                { "joules_per_kilogram_dry_air", "value * 2326" }, // Convert J/kg to BTU/lb
                { "joules_per_kilogram_degree_kelvin", "value * 4184" }, // Convert J/(kg·K) to BTU/lb
                { "kilojoules_per_degree_kelvin", "value * 4.184" } // Convert kJ/K to BTU/lb
            }
        );

        public static IotUnit joules_per_degree_kelvin => new IotUnit("Enthalpy", "joules_per_degree_kelvin", "J/K",
            new Dictionary<string, string>
            {
                { "btus_per_pound", "value / 4184" }, // Convert BTU/lb to J/K
                { "btus_per_pound_dry_air", "value / 4184" }, // Convert BTU/lb to J/K
                { "joules_per_kilogram_dry_air", "value * 1" }, // Convert J/kg to J/K
                { "joules_per_kilogram_degree_kelvin", "value * 1" }, // Convert J/(kg·K) to J/K
                { "kilojoules_per_degree_kelvin", "value / 1000" } // Convert kJ/K to J/K
            }
        );

        public static IotUnit joules_per_kilogram_dry_air => new IotUnit("Enthalpy", "joules_per_kilogram_dry_air", "J/kg",
            new Dictionary<string, string>
            {
                { "btus_per_pound", "value / 2326" }, // Convert BTU/lb to J/kg
                { "btus_per_pound_dry_air", "value / 2326" }, // Convert BTU/lb to J/kg
                { "joules_per_degree_kelvin", "value * 1" }, // Convert J/K to J/kg
                { "joules_per_kilogram_degree_kelvin", "value * 1" }, // Convert J/(kg·K) to J/kg
                { "kilojoules_per_kilogram_dry_air", "value / 1000" } // Convert kJ/kg to J/kg
            }
        );

        public static IotUnit joules_per_kilogram_degree_kelvin => new IotUnit("Enthalpy", "joules_per_kilogram_degree_kelvin", "J/(kg·K)",
            new Dictionary<string, string>
            {
                { "btus_per_pound", "value / 4184" }, // Convert BTU/lb to J/(kg·K)
                { "btus_per_pound_dry_air", "value / 4184" }, // Convert BTU/lb to J/(kg·K)
                { "joules_per_degree_kelvin", "value * 1" }, // Convert J/K to J/(kg·K)
                { "joules_per_kilogram_dry_air", "value * 1" }, // Convert J/kg to J/(kg·K)
                { "kilojoules_per_degree_kelvin", "value / 1000" } // Convert kJ/K to J/(kg·K)
            }
        );

        public static IotUnit kilojoules_per_degree_kelvin => new IotUnit("Enthalpy", "kilojoules_per_degree_kelvin", "kJ/K",
            new Dictionary<string, string>
            {
                { "btus_per_pound", "value / 4.184" }, // Convert BTU/lb to kJ/K
                { "btus_per_pound_dry_air", "value / 4.184" }, // Convert BTU/lb to kJ/K
                { "joules_per_degree_kelvin", "value * 1000" }, // Convert J/K to kJ/K
                { "joules_per_kilogram_dry_air", "value * 1000" }, // Convert J/kg to kJ/K
                { "joules_per_kilogram_degree_kelvin", "value * 1000" } // Convert J/(kg·K) to kJ/K
            }
        );

        public static IotUnit kilojoules_per_kilogram_dry_air => new IotUnit("Enthalpy", "kilojoules_per_kilogram_dry_air", "kJ/kg",
            new Dictionary<string, string>
            {
                { "btus_per_pound", "value / 2.326" }, // Convert BTU/lb to kJ/kg
                { "btus_per_pound_dry_air", "value / 2.326" }, // Convert BTU/lb to kJ/kg
                { "joules_per_degree_kelvin", "value * 1000" }, // Convert J/K to kJ/kg
                { "joules_per_kilogram_dry_air", "value * 1000" }, // Convert J/kg to kJ/kg
                { "kilojoules_per_degree_kelvin", "value * 1" } // Convert kJ/K to kJ/kg
            }
        );

        public static IotUnit megajoules_per_degree_kelvin => new IotUnit("Enthalpy", "megajoules_per_degree_kelvin", "MJ/K",
            new Dictionary<string, string>
            {
                { "btus_per_pound", "value / 4184e3" }, // Convert BTU/lb to MJ/K
                { "btus_per_pound_dry_air", "value / 4184e3" }, // Convert BTU/lb to MJ/K
                { "joules_per_degree_kelvin", "value * 1e6" }, // Convert J/K to MJ/K
                { "kilojoules_per_degree_kelvin", "value * 1000" }, // Convert kJ/K to MJ/K
                { "megajoules_per_kilogram_dry_air", "value * 1" } // Convert MJ/kg to MJ/K
            }
        );

        public static IotUnit megajoules_per_kilogram_dry_air => new IotUnit("Enthalpy", "megajoules_per_kilogram_dry_air", "MJ/kg",
            new Dictionary<string, string>
            {
                { "btus_per_pound", "value / 2326e3" }, // Convert BTU/lb to MJ/kg
                { "btus_per_pound_dry_air", "value / 2326e3" }, // Convert BTU/lb to MJ/kg
                { "joules_per_kilogram_dry_air", "value * 1e6" }, // Convert J/kg to MJ/kg
                { "kilojoules_per_kilogram_dry_air", "value * 1000" }, // Convert kJ/kg to MJ/kg
                { "megajoules_per_degree_kelvin", "value * 1" } // Convert MJ/K to MJ/kg
            }
        );



        // Force Units
        public static IotUnit newton => new IotUnit("Force", "newton", "N", null);

        // Frequency Units
        public static IotUnit cycles_per_hour => new IotUnit("Frequency", "cycles_per_hour", "cph",
            new Dictionary<string, string>
            {
                { "cycles_per_minute", "value / 60" }, // Convert cpm to cph
                { "hertz", "value / 3600" }, // Convert Hz to cph
                { "kilohertz", "value / 3.6e6" }, // Convert kHz to cph
                { "megahertz", "value / 3.6e9" }, // Convert MHz to cph
                { "per_hour", "value * 1" } // Convert ph to cph (identical)
            }
        );

        public static IotUnit cycles_per_minute => new IotUnit("Frequency", "cycles_per_minute", "cpm",
            new Dictionary<string, string>
            {
                { "cycles_per_hour", "value * 60" }, // Convert cph to cpm
                { "hertz", "value / 60" }, // Convert Hz to cpm
                { "kilohertz", "value / 60e3" }, // Convert kHz to cpm
                { "megahertz", "value / 60e6" }, // Convert MHz to cpm
                { "per_hour", "value * 60" } // Convert ph to cpm
            }
        );

        public static IotUnit hertz => new IotUnit("Frequency", "hertz", "Hz",
            new Dictionary<string, string>
            {
                { "cycles_per_hour", "value * 3600" }, // Convert cph to Hz
                { "cycles_per_minute", "value * 60" }, // Convert cpm to Hz
                { "kilohertz", "value / 1000" }, // Convert kHz to Hz
                { "megahertz", "value / 1e6" }, // Convert MHz to Hz
                { "per_hour", "value * 3600" } // Convert ph to Hz
            }
        );

        public static IotUnit kilohertz => new IotUnit("Frequency", "kilohertz", "kHz",
            new Dictionary<string, string>
            {
                { "cycles_per_hour", "value * 3.6e6" }, // Convert cph to kHz
                { "cycles_per_minute", "value * 60e3" }, // Convert cpm to kHz
                { "hertz", "value * 1000" }, // Convert Hz to kHz
                { "megahertz", "value / 1000" }, // Convert MHz to kHz
                { "per_hour", "value * 3.6e6" } // Convert ph to kHz
            }
        );

        public static IotUnit megahertz => new IotUnit("Frequency", "megahertz", "MHz",
            new Dictionary<string, string>
            {
                { "cycles_per_hour", "value * 3.6e9" }, // Convert cph to MHz
                { "cycles_per_minute", "value * 60e6" }, // Convert cpm to MHz
                { "hertz", "value * 1e6" }, // Convert Hz to MHz
                { "kilohertz", "value * 1000" }, // Convert kHz to MHz
                { "per_hour", "value * 3.6e9" } // Convert ph to MHz
            }
        );

        public static IotUnit per_hour => new IotUnit("Frequency", "per_hour", "ph",
            new Dictionary<string, string>
            {
                { "cycles_per_hour", "value * 1" }, // Convert cph to ph (identical)
                { "cycles_per_minute", "value / 60" }, // Convert cpm to ph
                { "hertz", "value / 3600" }, // Convert Hz to ph
                { "kilohertz", "value / 3.6e6" }, // Convert kHz to ph
                { "megahertz", "value / 3.6e9" } // Convert MHz to ph
            }
        );


        // General Units
        public static IotUnit decibels_a => new IotUnit("General", "decibels_a", "dBA", null);
        public static IotUnit grams_per_square_meter => new IotUnit("General", "grams_per_square_meter", "g/m²", null);
        public static IotUnit nephelometric_turbidity_unit => new IotUnit("General", "nephelometric_turbidity_unit", "NTU", null);
        public static IotUnit pH => new IotUnit("General", "pH", "pH", null);

        // Humidity Units
        public static IotUnit grams_of_water_per_kilogram_dry_air => new IotUnit("Humidity", "grams_of_water_per_kilogram_dry_air", "g/kg",
            new Dictionary<string, string>
            {
                { "percent_relative_humidity", "value * 0.622" } // Convert %RH to g/kg (approximation based on psychrometric principles)
            }
        );

        public static IotUnit percent_relative_humidity => new IotUnit("Humidity", "percent_relative_humidity", "%RH",
            new Dictionary<string, string>
            {
                { "grams_of_water_per_kilogram_dry_air", "value / 0.622" } // Convert g/kg to %RH (approximation)
            }
        );


        // Illuminance Units
        public static IotUnit foot_candles => new IotUnit("Illuminance", "foot_candles", "fc",
            new Dictionary<string, string>
            {
                { "lux", "value * 10.764" } // Convert lx to fc
            }
        );

        public static IotUnit lux => new IotUnit("Illuminance", "lux", "lx",
            new Dictionary<string, string>
            {
                { "foot_candles", "value / 10.764" } // Convert fc to lx
            }
        );


        // Inductance Units
        public static IotUnit henrys_inductance => new IotUnit("Inductance", "henrys", "H",
            new Dictionary<string, string>
            {
                { "microhenrys_inductance", "value * 1e6" }, // Convert µH to H
                { "millihenrys_inductance", "value * 1000" } // Convert mH to H
            }
        );

        public static IotUnit microhenrys_inductance => new IotUnit("Inductance", "microhenrys", "µH",
            new Dictionary<string, string>
            {
                { "henrys_inductance", "value / 1e6" }, // Convert H to µH
                { "millihenrys_inductance", "value / 1000" } // Convert mH to µH
            }
        );

        public static IotUnit millihenrys_inductance => new IotUnit("Inductance", "millihenrys", "mH",
            new Dictionary<string, string>
            {
                { "henrys_inductance", "value / 1000" }, // Convert H to mH
                { "microhenrys_inductance", "value * 1000" } // Convert µH to mH
            }
        );


        // Length Units
        public static IotUnit centimeters => new IotUnit("Length", "centimeters", "cm",
            new Dictionary<string, string>
            {
                { "feet", "value / 30.48" }, // Convert ft to cm
                { "inches", "value / 2.54" }, // Convert in to cm
                { "kilometers", "value / 100000" }, // Convert km to cm
                { "meters", "value / 100" }, // Convert m to cm
                { "micrometers", "value * 10000" }, // Convert µm to cm
                { "millimeters", "value * 10" } // Convert mm to cm
            }
        );

        public static IotUnit feet => new IotUnit("Length", "feet", "ft",
            new Dictionary<string, string>
            {
                { "centimeters", "value * 30.48" }, // Convert cm to ft
                { "inches", "value * 12" }, // Convert in to ft
                { "kilometers", "value / 3280.84" }, // Convert km to ft
                { "meters", "value / 3.28084" }, // Convert m to ft
                { "micrometers", "value * 304800" }, // Convert µm to ft
                { "millimeters", "value * 304.8" } // Convert mm to ft
            }
        );

        public static IotUnit inches => new IotUnit("Length", "inches", "in",
            new Dictionary<string, string>
            {
                { "centimeters", "value * 2.54" }, // Convert cm to in
                { "feet", "value / 12" }, // Convert ft to in
                { "kilometers", "value / 39370.1" }, // Convert km to in
                { "meters", "value / 39.3701" }, // Convert m to in
                { "micrometers", "value * 25400" }, // Convert µm to in
                { "millimeters", "value * 25.4" } // Convert mm to in
            }
        );

        public static IotUnit kilometers => new IotUnit("Length", "kilometers", "km",
            new Dictionary<string, string>
            {
                { "centimeters", "value * 100000" }, // Convert cm to km
                { "feet", "value * 3280.84" }, // Convert ft to km
                { "inches", "value * 39370.1" }, // Convert in to km
                { "meters", "value * 1000" }, // Convert m to km
                { "micrometers", "value * 1e9" }, // Convert µm to km
                { "millimeters", "value * 1e6" } // Convert mm to km
            }
        );

        public static IotUnit meters => new IotUnit("Length", "meters", "m",
            new Dictionary<string, string>
            {
                { "centimeters", "value * 100" }, // Convert cm to m
                { "feet", "value * 3.28084" }, // Convert ft to m
                { "inches", "value * 39.3701" }, // Convert in to m
                { "kilometers", "value / 1000" }, // Convert km to m
                { "micrometers", "value * 1e6" }, // Convert µm to m
                { "millimeters", "value * 1000" } // Convert mm to m
            }
        );

        public static IotUnit micrometers => new IotUnit("Length", "micrometers", "µm",
            new Dictionary<string, string>
            {
                { "centimeters", "value / 10000" }, // Convert cm to µm
                { "feet", "value / 304800" }, // Convert ft to µm
                { "inches", "value / 25400" }, // Convert in to µm
                { "kilometers", "value / 1e9" }, // Convert km to µm
                { "meters", "value / 1e6" }, // Convert m to µm
                { "millimeters", "value / 1000" } // Convert mm to µm
            }
        );

        public static IotUnit millimeters => new IotUnit("Length", "millimeters", "mm",
            new Dictionary<string, string>
            {
                { "centimeters", "value / 10" }, // Convert cm to mm
                { "feet", "value / 304.8" }, // Convert ft to mm
                { "inches", "value / 25.4" }, // Convert in to mm
                { "kilometers", "value / 1e6" }, // Convert km to mm
                { "meters", "value / 1000" }, // Convert m to mm
                { "micrometers", "value * 1000" } // Convert µm to mm
            }
        );


        // Light Units
        public static IotUnit candelas => new IotUnit("Light", "candelas", "cd",
            new Dictionary<string, string>
            {
                { "candelas_per_square_meter", "value * 1" }, // Convert cd/m² to cd (placeholder for identical units)
                { "lumens", "value * 4 * Math.PI" }, // Convert cd to lm (approximation for isotropic source)
                { "luxes", "value * 4 * Math.PI" }, // Convert cd to lx (same as lumens in general lighting)
                { "watts_per_square_foot", "value * 0.092903 * 4 * Math.PI" }, // Approximation for W/ft² to cd
                { "watts_per_square_meter", "value * 4 * Math.PI" } // Convert W/m² to cd
            }
        );

        public static IotUnit candelas_per_square_meter => new IotUnit("Light", "candelas_per_square_meter", "cd/m²",
            new Dictionary<string, string>
            {
                { "candelas", "value * 1" }, // Convert cd to cd/m²
                { "lumens", "value * 4 * Math.PI" }, // Convert cd/m² to lm (approximation)
                { "luxes", "value * 1" }, // Convert lx to cd/m²
                { "watts_per_square_foot", "value * 0.092903" }, // Convert W/ft² to cd/m²
                { "watts_per_square_meter", "value * 1" } // Convert W/m² to cd/m²
            }
        );

        public static IotUnit foot_candles_light => new IotUnit("Light", "foot_candles", "fc",
            new Dictionary<string, string>
            {
                { "luxes", "value * 10.764" }, // Convert lx to fc
                { "candelas", "value / 10.764" }, // Convert cd to fc (approximation)
                { "watts_per_square_foot", "value * 1" }, // Convert W/ft² to fc
                { "watts_per_square_meter", "value * 10.764" }, // Convert W/m² to fc
                { "lumens", "value * 1" } // Convert lm to fc
            }
        );

        public static IotUnit lumens => new IotUnit("Light", "lumens", "lm",
            new Dictionary<string, string>
            {
                { "candelas", "value / (4 * Math.PI)" }, // Convert cd to lm (approximation)
                { "luxes", "value * 1" }, // Convert lx to lm
                { "watts_per_square_foot", "value * 1" }, // Convert W/ft² to lm
                { "watts_per_square_meter", "value * 1" }, // Convert W/m² to lm
                { "candelas_per_square_meter", "value / (4 * Math.PI)" } // Convert cd/m² to lm
            }
        );

        public static IotUnit luxes => new IotUnit("Light", "luxes", "lx",
            new Dictionary<string, string>
            {
                { "candelas", "value / (4 * Math.PI)" }, // Convert cd to lx
                { "candelas_per_square_meter", "value * 1" }, // Convert cd/m² to lx
                { "foot_candles_light", "value / 10.764" }, // Convert fc to lx
                { "watts_per_square_foot", "value / 10.764" }, // Convert W/ft² to lx
                { "watts_per_square_meter", "value * 1" } // Convert W/m² to lx
            }
        );

        public static IotUnit watts_per_square_foot => new IotUnit("Light", "watts_per_square_foot", "W/ft²",
            new Dictionary<string, string>
            {
                { "candelas", "value / 0.092903" }, // Convert cd to W/ft²
                { "candelas_per_square_meter", "value / 0.092903" }, // Convert cd/m² to W/ft²
                { "luxes", "value * 10.764" }, // Convert lx to W/ft²
                { "foot_candles_light", "value * 1" }, // Convert fc to W/ft²
                { "watts_per_square_meter", "value * 10.764" } // Convert W/m² to W/ft²
            }
        );

        public static IotUnit watts_per_square_meter => new IotUnit("Light", "watts_per_square_meter", "W/m²",
            new Dictionary<string, string>
            {
                { "candelas", "value / (4 * Math.PI)" }, // Convert cd to W/m²
                { "candelas_per_square_meter", "value * 1" }, // Convert cd/m² to W/m²
                { "luxes", "value * 1" }, // Convert lx to W/m²
                { "foot_candles_light", "value / 10.764" }, // Convert fc to W/m²
                { "watts_per_square_foot", "value / 10.764" } // Convert W/ft² to W/m²
            }
        );


        // Luminance Units
        public static IotUnit candelas_per_square_meter_luminance => new IotUnit("Luminance", "candelas_per_square_meter", "cd/m²",
            new Dictionary<string, string>
            {
                { "nits", "value * 1" } // Convert nits to cd/m² (identical)
            }
        );

        public static IotUnit nits => new IotUnit("Luminance", "nits", "nt",
            new Dictionary<string, string>
            {
                { "candelas_per_square_meter_luminance", "value * 1" } // Convert cd/m² to nits (identical)
            }
        );


        // Luminous Intensity Units
        public static IotUnit candela => new IotUnit("LuminousIntensity", "candela", "cd", null);

        // Magnetic Field Strength Units
        public static IotUnit amperes_per_meter_magnetic_field => new IotUnit("MagneticFieldStrength", "amperes_per_meter", "A/m",
            new Dictionary<string, string>
            {
                { "oersteds", "value / 79.57747154594767" } // Convert Oe to A/m
            }
        );

        public static IotUnit oersteds => new IotUnit("MagneticFieldStrength", "oersteds", "Oe",
            new Dictionary<string, string>
            {
                { "amperes_per_meter_magnetic_field", "value * 79.57747154594767" } // Convert A/m to Oe
            }
        );


        // Magnetic Flux Units
        public static IotUnit maxwells => new IotUnit("MagneticFlux", "maxwells", "Mx",
            new Dictionary<string, string>
            {
                { "webers", "value / 1e8" } // Convert Wb to Mx
            }
        );

        public static IotUnit webers => new IotUnit("MagneticFlux", "webers", "Wb",
            new Dictionary<string, string>
            {
                { "maxwells", "value * 1e8" } // Convert Mx to Wb
            }
        );


        // Mass Units
        public static IotUnit grams => new IotUnit("Mass", "grams", "g",
            new Dictionary<string, string>
            {
                { "kilograms", "value / 1000" }, // Convert kg to g
                { "milligrams", "value * 1000" }, // Convert mg to g
                { "pounds_mass", "value / 453.59237" }, // Convert lb to g
                { "tons", "value / 1e6" } // Convert t to g
            }
        );

        public static IotUnit kilograms => new IotUnit("Mass", "kilograms", "kg",
            new Dictionary<string, string>
            {
                { "grams", "value * 1000" }, // Convert g to kg
                { "milligrams", "value * 1e6" }, // Convert mg to kg
                { "pounds_mass", "value * 2.20462" }, // Convert lb to kg
                { "tons", "value / 1000" } // Convert t to kg
            }
        );

        public static IotUnit milligrams => new IotUnit("Mass", "milligrams", "mg",
            new Dictionary<string, string>
            {
                { "grams", "value / 1000" }, // Convert g to mg
                { "kilograms", "value / 1e6" }, // Convert kg to mg
                { "pounds_mass", "value / 453592.37" }, // Convert lb to mg
                { "tons", "value / 1e9" } // Convert t to mg
            }
        );

        public static IotUnit pounds_mass => new IotUnit("Mass", "pounds_mass", "lb",
            new Dictionary<string, string>
            {
                { "grams", "value * 453.59237" }, // Convert g to lb
                { "kilograms", "value / 2.20462" }, // Convert kg to lb
                { "milligrams", "value * 453592.37" }, // Convert mg to lb
                { "tons", "value / 2204.62" } // Convert t to lb
            }
        );

        public static IotUnit tons => new IotUnit("Mass", "tons", "t",
            new Dictionary<string, string>
            {
                { "grams", "value * 1e6" }, // Convert g to t
                { "kilograms", "value * 1000" }, // Convert kg to t
                { "milligrams", "value * 1e9" }, // Convert mg to t
                { "pounds_mass", "value * 2204.62" } // Convert lb to t
            }
        );


        // Mass Density Units
        public static IotUnit grams_per_cubic_centimeter => new IotUnit("MassDensity", "grams_per_cubic_centimeter", "g/cm³",
            new Dictionary<string, string>
            {
                { "grams_per_cubic_meter", "value * 1e6" }, // Convert g/m³ to g/cm³
                { "kilograms_per_cubic_meter", "value * 1000" }, // Convert kg/m³ to g/cm³
                { "micrograms_per_cubic_meter", "value * 1e9" }, // Convert µg/m³ to g/cm³
                { "milligrams_per_cubic_meter", "value * 1e6" }, // Convert mg/m³ to g/cm³
                { "nanograms_per_cubic_meter", "value * 1e12" } // Convert ng/m³ to g/cm³
            }
        );

        public static IotUnit grams_per_cubic_meter => new IotUnit("MassDensity", "grams_per_cubic_meter", "g/m³",
            new Dictionary<string, string>
            {
                { "grams_per_cubic_centimeter", "value / 1e6" }, // Convert g/cm³ to g/m³
                { "kilograms_per_cubic_meter", "value / 1000" }, // Convert kg/m³ to g/m³
                { "micrograms_per_cubic_meter", "value * 1000" }, // Convert µg/m³ to g/m³
                { "milligrams_per_cubic_meter", "value * 1" }, // Convert mg/m³ to g/m³
                { "nanograms_per_cubic_meter", "value * 1e6" } // Convert ng/m³ to g/m³
            }
        );

        public static IotUnit kilograms_per_cubic_meter => new IotUnit("MassDensity", "kilograms_per_cubic_meter", "kg/m³",
            new Dictionary<string, string>
            {
                { "grams_per_cubic_centimeter", "value / 1000" }, // Convert g/cm³ to kg/m³
                { "grams_per_cubic_meter", "value * 1000" }, // Convert g/m³ to kg/m³
                { "micrograms_per_cubic_meter", "value * 1e9" }, // Convert µg/m³ to kg/m³
                { "milligrams_per_cubic_meter", "value * 1e6" }, // Convert mg/m³ to kg/m³
                { "nanograms_per_cubic_meter", "value * 1e12" } // Convert ng/m³ to kg/m³
            }
        );

        public static IotUnit micrograms_per_cubic_meter => new IotUnit("MassDensity", "micrograms_per_cubic_meter", "µg/m³",
            new Dictionary<string, string>
            {
                { "grams_per_cubic_centimeter", "value / 1e9" }, // Convert g/cm³ to µg/m³
                { "grams_per_cubic_meter", "value / 1000" }, // Convert g/m³ to µg/m³
                { "kilograms_per_cubic_meter", "value / 1e9" }, // Convert kg/m³ to µg/m³
                { "milligrams_per_cubic_meter", "value / 1000" }, // Convert mg/m³ to µg/m³
                { "nanograms_per_cubic_meter", "value * 1000" } // Convert ng/m³ to µg/m³
            }
        );

        public static IotUnit milligrams_per_cubic_meter => new IotUnit("MassDensity", "milligrams_per_cubic_meter", "mg/m³",
            new Dictionary<string, string>
            {
                { "grams_per_cubic_centimeter", "value / 1e6" }, // Convert g/cm³ to mg/m³
                { "grams_per_cubic_meter", "value * 1" }, // Convert g/m³ to mg/m³
                { "kilograms_per_cubic_meter", "value / 1000" }, // Convert kg/m³ to mg/m³
                { "micrograms_per_cubic_meter", "value * 1000" }, // Convert µg/m³ to mg/m³
                { "nanograms_per_cubic_meter", "value * 1e6" } // Convert ng/m³ to mg/m³
            }
        );

        public static IotUnit nanograms_per_cubic_meter => new IotUnit("MassDensity", "nanograms_per_cubic_meter", "ng/m³",
            new Dictionary<string, string>
            {
                { "grams_per_cubic_centimeter", "value / 1e12" }, // Convert g/cm³ to ng/m³
                { "grams_per_cubic_meter", "value / 1e6" }, // Convert g/m³ to ng/m³
                { "kilograms_per_cubic_meter", "value / 1e12" }, // Convert kg/m³ to ng/m³
                { "micrograms_per_cubic_meter", "value / 1000" }, // Convert µg/m³ to ng/m³
                { "milligrams_per_cubic_meter", "value / 1e6" } // Convert mg/m³ to ng/m³
            }
        );


        // Mass Flow Units
        public static IotUnit grams_per_minute => new IotUnit("MassFlow", "grams_per_minute", "g/min",
            new Dictionary<string, string>
            {
                { "grams_per_second", "value / 60" }, // Convert g/s to g/min
                { "kilograms_per_hour", "value / 1000 * 60" }, // Convert kg/h to g/min
                { "kilograms_per_minute", "value / 1000" }, // Convert kg/min to g/min
                { "kilograms_per_second", "value / 1000 / 60" }, // Convert kg/s to g/min
                { "pounds_mass_per_hour", "value / 453.59237 * 60" }, // Convert lb/h to g/min
                { "pounds_mass_per_minute", "value / 453.59237" }, // Convert lb/min to g/min
                { "pounds_mass_per_second", "value / 453.59237 / 60" }, // Convert lb/s to g/min
                { "tons_per_hour", "value / 1e6 * 60" } // Convert t/h to g/min
            }
        );

        public static IotUnit grams_per_second => new IotUnit("MassFlow", "grams_per_second", "g/s",
            new Dictionary<string, string>
            {
                { "grams_per_minute", "value * 60" }, // Convert g/min to g/s
                { "kilograms_per_hour", "value / 1000 * 3600" }, // Convert kg/h to g/s
                { "kilograms_per_minute", "value / 1000 * 60" }, // Convert kg/min to g/s
                { "kilograms_per_second", "value / 1000" }, // Convert kg/s to g/s
                { "pounds_mass_per_hour", "value / 453.59237 * 3600" }, // Convert lb/h to g/s
                { "pounds_mass_per_minute", "value / 453.59237 * 60" }, // Convert lb/min to g/s
                { "pounds_mass_per_second", "value / 453.59237" }, // Convert lb/s to g/s
                { "tons_per_hour", "value / 1e6 * 3600" } // Convert t/h to g/s
            }
        );

        public static IotUnit kilograms_per_hour => new IotUnit("MassFlow", "kilograms_per_hour", "kg/h",
            new Dictionary<string, string>
            {
                { "grams_per_minute", "value * 1000 / 60" }, // Convert g/min to kg/h
                { "grams_per_second", "value * 1000 / 3600" }, // Convert g/s to kg/h
                { "kilograms_per_minute", "value / 60" }, // Convert kg/min to kg/h
                { "kilograms_per_second", "value / 3600" }, // Convert kg/s to kg/h
                { "pounds_mass_per_hour", "value * 2.20462" }, // Convert lb/h to kg/h
                { "pounds_mass_per_minute", "value * 2.20462 / 60" }, // Convert lb/min to kg/h
                { "pounds_mass_per_second", "value * 2.20462 / 3600" }, // Convert lb/s to kg/h
                { "tons_per_hour", "value / 1000" } // Convert t/h to kg/h
            }
        );

        public static IotUnit kilograms_per_minute => new IotUnit("MassFlow", "kilograms_per_minute", "kg/min",
            new Dictionary<string, string>
            {
                { "grams_per_minute", "value * 1000" }, // Convert g/min to kg/min
                { "grams_per_second", "value * 1000 / 60" }, // Convert g/s to kg/min
                { "kilograms_per_hour", "value * 60" }, // Convert kg/h to kg/min
                { "kilograms_per_second", "value / 60" }, // Convert kg/s to kg/min
                { "pounds_mass_per_hour", "value * 2.20462 * 60" }, // Convert lb/h to kg/min
                { "pounds_mass_per_minute", "value * 2.20462" }, // Convert lb/min to kg/min
                { "pounds_mass_per_second", "value * 2.20462 / 60" }, // Convert lb/s to kg/min
                { "tons_per_hour", "value / 16.6667" } // Convert t/h to kg/min
            }
        );

        public static IotUnit kilograms_per_second => new IotUnit("MassFlow", "kilograms_per_second", "kg/s",
            new Dictionary<string, string>
            {
                { "grams_per_minute", "value * 1000 * 60" }, // Convert g/min to kg/s
                { "grams_per_second", "value * 1000" }, // Convert g/s to kg/s
                { "kilograms_per_hour", "value * 3600" }, // Convert kg/h to kg/s
                { "kilograms_per_minute", "value * 60" }, // Convert kg/min to kg/s
                { "pounds_mass_per_hour", "value * 2.20462 * 3600" }, // Convert lb/h to kg/s
                { "pounds_mass_per_minute", "value * 2.20462 * 60" }, // Convert lb/min to kg/s
                { "pounds_mass_per_second", "value * 2.20462" }, // Convert lb/s to kg/s
                { "tons_per_hour", "value / 0.27778" } // Convert t/h to kg/s
            }
        );

        public static IotUnit pounds_mass_per_hour => new IotUnit("MassFlow", "pounds_mass_per_hour", "lb/h",
            new Dictionary<string, string>
            {
                { "grams_per_minute", "value * 453.59237 / 60" }, // Convert g/min to lb/h
                { "grams_per_second", "value * 453.59237 / 3600" }, // Convert g/s to lb/h
                { "kilograms_per_hour", "value / 2.20462" }, // Convert kg/h to lb/h
                { "kilograms_per_minute", "value / 2.20462 / 60" }, // Convert kg/min to lb/h
                { "kilograms_per_second", "value / 2.20462 / 3600" }, // Convert kg/s to lb/h
                { "pounds_mass_per_minute", "value / 60" }, // Convert lb/min to lb/h
                { "pounds_mass_per_second", "value / 3600" }, // Convert lb/s to lb/h
                { "tons_per_hour", "value / 2204.62" } // Convert t/h to lb/h
            }
        );

        public static IotUnit pounds_mass_per_minute => new IotUnit("MassFlow", "pounds_mass_per_minute", "lb/min",
            new Dictionary<string, string>
            {
                { "grams_per_minute", "value * 453.59237" }, // Convert g/min to lb/min
                { "grams_per_second", "value * 453.59237 / 60" }, // Convert g/s to lb/min
                { "kilograms_per_hour", "value * 2.20462 * 60" }, // Convert kg/h to lb/min
                { "kilograms_per_minute", "value * 2.20462" }, // Convert kg/min to lb/min
                { "kilograms_per_second", "value * 2.20462 / 60" }, // Convert kg/s to lb/min
                { "pounds_mass_per_hour", "value * 60" }, // Convert lb/h to lb/min
                { "pounds_mass_per_second", "value / 60" }, // Convert lb/s to lb/min
                { "tons_per_hour", "value / 36.7437" } // Convert t/h to lb/min
            }
        );

        public static IotUnit pounds_mass_per_second => new IotUnit("MassFlow", "pounds_mass_per_second", "lb/s",
            new Dictionary<string, string>
            {
                { "grams_per_minute", "value * 453.59237 * 60" }, // Convert g/min to lb/s
                { "grams_per_second", "value * 453.59237" }, // Convert g/s to lb/s
                { "kilograms_per_hour", "value * 2.20462 * 3600" }, // Convert kg/h to lb/s
                { "kilograms_per_minute", "value * 2.20462 * 60" }, // Convert kg/min to lb/s
                { "kilograms_per_second", "value * 2.20462" }, // Convert kg/s to lb/s
                { "pounds_mass_per_hour", "value * 3600" }, // Convert lb/h to lb/s
                { "pounds_mass_per_minute", "value * 60" }, // Convert lb/min to lb/s
                { "tons_per_hour", "value / 0.00027778" } // Convert t/h to lb/s
            }
        );

        public static IotUnit tons_per_hour => new IotUnit("MassFlow", "tons_per_hour", "t/h",
            new Dictionary<string, string>
            {
                { "grams_per_minute", "value * 1e6 / 60" }, // Convert g/min to t/h
                { "grams_per_second", "value * 1e6 / 3600" }, // Convert g/s to t/h
                { "kilograms_per_hour", "value * 1000" }, // Convert kg/h to t/h
                { "kilograms_per_minute", "value * 16.6667" }, // Convert kg/min to t/h
                { "kilograms_per_second", "value * 0.27778" }, // Convert kg/s to t/h
                { "pounds_mass_per_hour", "value * 2204.62" }, // Convert lb/h to t/h
                { "pounds_mass_per_minute", "value * 36.7437" }, // Convert lb/min to t/h
                { "pounds_mass_per_second", "value * 0.611157" } // Convert lb/s to t/h
            }
        );


        // Mass Fraction Units
        public static IotUnit grams_per_gram => new IotUnit("MassFraction", "grams_per_gram", "g/g",
            new Dictionary<string, string>
            {
                { "grams_per_kilogram", "value * 1000" }, // Convert g/kg to g/g
                { "grams_per_liter", "value * 1000" }, // Convert g/L to g/g
                { "grams_per_milliliter", "value * 1" }, // Convert g/mL to g/g
                { "kilograms_per_kilogram", "value / 1000" }, // Convert kg/kg to g/g
                { "micrograms_per_liter", "value * 1e9" }, // Convert µg/L to g/g
                { "milligrams_per_gram", "value * 1000" }, // Convert mg/g to g/g
                { "milligrams_per_kilogram", "value * 1e6" }, // Convert mg/kg to g/g
                { "milligrams_per_liter", "value * 1e6" } // Convert mg/L to g/g
            }
        );

        public static IotUnit grams_per_kilogram => new IotUnit("MassFraction", "grams_per_kilogram", "g/kg",
            new Dictionary<string, string>
            {
                { "grams_per_gram", "value / 1000" }, // Convert g/g to g/kg
                { "grams_per_liter", "value * 1" }, // Convert g/L to g/kg
                { "grams_per_milliliter", "value / 1000" }, // Convert g/mL to g/kg
                { "kilograms_per_kilogram", "value / 1000" }, // Convert kg/kg to g/kg
                { "micrograms_per_liter", "value * 1e6" }, // Convert µg/L to g/kg
                { "milligrams_per_gram", "value * 1" }, // Convert mg/g to g/kg
                { "milligrams_per_kilogram", "value * 1000" }, // Convert mg/kg to g/kg
                { "milligrams_per_liter", "value * 1000" } // Convert mg/L to g/kg
            }
        );

        public static IotUnit grams_per_liter => new IotUnit("MassFraction", "grams_per_liter", "g/L",
            new Dictionary<string, string>
            {
                { "grams_per_gram", "value / 1000" }, // Convert g/g to g/L
                { "grams_per_kilogram", "value * 1" }, // Convert g/kg to g/L
                { "grams_per_milliliter", "value / 1000" }, // Convert g/mL to g/L
                { "kilograms_per_kilogram", "value / 1000" }, // Convert kg/kg to g/L
                { "micrograms_per_liter", "value * 1e6" }, // Convert µg/L to g/L
                { "milligrams_per_gram", "value * 1" }, // Convert mg/g to g/L
                { "milligrams_per_kilogram", "value * 1000" }, // Convert mg/kg to g/L
                { "milligrams_per_liter", "value * 1000" } // Convert mg/L to g/L
            }
        );

        public static IotUnit grams_per_milliliter => new IotUnit("MassFraction", "grams_per_milliliter", "g/mL",
            new Dictionary<string, string>
            {
                { "grams_per_gram", "value * 1" }, // Convert g/g to g/mL
                { "grams_per_kilogram", "value * 1000" }, // Convert g/kg to g/mL
                { "grams_per_liter", "value * 1000" }, // Convert g/L to g/mL
                { "kilograms_per_kilogram", "value * 1" }, // Convert kg/kg to g/mL
                { "micrograms_per_liter", "value * 1e9" }, // Convert µg/L to g/mL
                { "milligrams_per_gram", "value * 1000" }, // Convert mg/g to g/mL
                { "milligrams_per_kilogram", "value * 1e6" }, // Convert mg/kg to g/mL
                { "milligrams_per_liter", "value * 1e6" } // Convert mg/L to g/mL
            }
        );

        public static IotUnit kilograms_per_kilogram => new IotUnit("MassFraction", "kilograms_per_kilogram", "kg/kg",
            new Dictionary<string, string>
            {
                { "grams_per_gram", "value * 1000" }, // Convert g/g to kg/kg
                { "grams_per_kilogram", "value * 1000" }, // Convert g/kg to kg/kg
                { "grams_per_liter", "value * 1000" }, // Convert g/L to kg/kg
                { "grams_per_milliliter", "value * 1" }, // Convert g/mL to kg/kg
                { "micrograms_per_liter", "value * 1e12" }, // Convert µg/L to kg/kg
                { "milligrams_per_gram", "value * 1e6" }, // Convert mg/g to kg/kg
                { "milligrams_per_kilogram", "value * 1e9" }, // Convert mg/kg to kg/kg
                { "milligrams_per_liter", "value * 1e9" } // Convert mg/L to kg/kg
            }
        );

        public static IotUnit micrograms_per_liter => new IotUnit("MassFraction", "micrograms_per_liter", "µg/L",
            new Dictionary<string, string>
            {
                { "grams_per_gram", "value / 1e9" }, // Convert g/g to µg/L
                { "grams_per_kilogram", "value / 1e6" }, // Convert g/kg to µg/L
                { "grams_per_liter", "value / 1e6" }, // Convert g/L to µg/L
                { "grams_per_milliliter", "value / 1e9" }, // Convert g/mL to µg/L
                { "kilograms_per_kilogram", "value / 1e12" }, // Convert kg/kg to µg/L
                { "milligrams_per_gram", "value / 1e6" }, // Convert mg/g to µg/L
                { "milligrams_per_kilogram", "value / 1000" }, // Convert mg/kg to µg/L
                { "milligrams_per_liter", "value * 1000" } // Convert mg/L to µg/L
            }
        );

        public static IotUnit milligrams_per_gram => new IotUnit("MassFraction", "milligrams_per_gram", "mg/g",
            new Dictionary<string, string>
            {
                { "grams_per_gram", "value / 1000" }, // Convert g/g to mg/g
                { "grams_per_kilogram", "value / 1e6" }, // Convert g/kg to mg/g
                { "grams_per_liter", "value / 1e6" }, // Convert g/L to mg/g
                { "grams_per_milliliter", "value / 1e3" }, // Convert g/mL to mg/g
                { "kilograms_per_kilogram", "value / 1e6" }, // Convert kg/kg to mg/g
                { "micrograms_per_liter", "value * 1000" }, // Convert µg/L to mg/g
                { "milligrams_per_kilogram", "value / 1000" }, // Convert mg/kg to mg/g
                { "milligrams_per_liter", "value / 1000" } // Convert mg/L to mg/g
            }
        );

        public static IotUnit milligrams_per_kilogram => new IotUnit("MassFraction", "milligrams_per_kilogram", "mg/kg",
            new Dictionary<string, string>
            {
                { "grams_per_gram", "value / 1e6" }, // Convert g/g to mg/kg
                { "grams_per_kilogram", "value / 1000" }, // Convert g/kg to mg/kg
                { "grams_per_liter", "value / 1000" }, // Convert g/L to mg/kg
                { "grams_per_milliliter", "value / 1e6" }, // Convert g/mL to mg/kg
                { "kilograms_per_kilogram", "value / 1e9" }, // Convert kg/kg to mg/kg
                { "micrograms_per_liter", "value * 1000" }, // Convert µg/L to mg/kg
                { "milligrams_per_gram", "value * 1000" }, // Convert mg/g to mg/kg
                { "milligrams_per_liter", "value / 1" } // Convert mg/L to mg/kg
            }
        );

        public static IotUnit milligrams_per_liter => new IotUnit("MassFraction", "milligrams_per_liter", "mg/L",
            new Dictionary<string, string>
            {
                { "grams_per_gram", "value / 1e6" }, // Convert g/g to mg/L
                { "grams_per_kilogram", "value / 1000" }, // Convert g/kg to mg/L
                { "grams_per_liter", "value / 1000" }, // Convert g/L to mg/L
                { "grams_per_milliliter", "value / 1e6" }, // Convert g/mL to mg/L
                { "kilograms_per_kilogram", "value / 1e9" }, // Convert kg/kg to mg/L
                { "micrograms_per_liter", "value / 1000" }, // Convert µg/L to mg/L
                { "milligrams_per_gram", "value / 1" }, // Convert mg/g to mg/L
                { "milligrams_per_kilogram", "value / 1" } // Convert mg/kg to mg/L
            }
        );


        // Physical Properties Units
        public static IotUnit newton_seconds => new IotUnit("PhysicalProperties", "newton_seconds", "N·s",
            new Dictionary<string, string>
            {
                { "newtons_per_meter", "value * 1" }, // Convert N/m to N·s (placeholder, specific logic needed)
                { "pascal_seconds", "value * 1" }, // Convert Pa·s to N·s (placeholder)
                { "square_meters_per_newton", "value * 1" }, // Convert m²/N to N·s (placeholder)
                { "watts_per_meter_per_degree_kelvin", "value * 1" }, // Convert W/(m·K) to N·s (placeholder)
                { "watts_per_square_meter_degree_kelvin", "value * 1" } // Convert W/(m²·K) to N·s (placeholder)
            }
        );

        public static IotUnit newtons_per_meter => new IotUnit("PhysicalProperties", "newtons_per_meter", "N/m",
            new Dictionary<string, string>
            {
                { "newton_seconds", "value * 1" }, // Convert N·s to N/m (placeholder)
                { "pascal_seconds", "value * 1" }, // Convert Pa·s to N/m (placeholder)
                { "square_meters_per_newton", "value * 1" }, // Convert m²/N to N/m (placeholder)
                { "watts_per_meter_per_degree_kelvin", "value * 1" }, // Convert W/(m·K) to N/m (placeholder)
                { "watts_per_square_meter_degree_kelvin", "value * 1" } // Convert W/(m²·K) to N/m (placeholder)
            }
        );

        public static IotUnit pascal_seconds => new IotUnit("PhysicalProperties", "pascal_seconds", "Pa·s",
            new Dictionary<string, string>
            {
                { "newton_seconds", "value * 1" }, // Convert N·s to Pa·s (placeholder)
                { "newtons_per_meter", "value * 1" }, // Convert N/m to Pa·s (placeholder)
                { "square_meters_per_newton", "value * 1" }, // Convert m²/N to Pa·s (placeholder)
                { "watts_per_meter_per_degree_kelvin", "value * 1" }, // Convert W/(m·K) to Pa·s (placeholder)
                { "watts_per_square_meter_degree_kelvin", "value * 1" } // Convert W/(m²·K) to Pa·s (placeholder)
            }
        );

        public static IotUnit square_meters_per_newton => new IotUnit("PhysicalProperties", "square_meters_per_newton", "m²/N",
            new Dictionary<string, string>
            {
                { "newton_seconds", "value * 1" }, // Convert N·s to m²/N (placeholder)
                { "newtons_per_meter", "value * 1" }, // Convert N/m to m²/N (placeholder)
                { "pascal_seconds", "value * 1" }, // Convert Pa·s to m²/N (placeholder)
                { "watts_per_meter_per_degree_kelvin", "value * 1" }, // Convert W/(m·K) to m²/N (placeholder)
                { "watts_per_square_meter_degree_kelvin", "value * 1" } // Convert W/(m²·K) to m²/N (placeholder)
            }
        );

        public static IotUnit watts_per_meter_per_degree_kelvin => new IotUnit("PhysicalProperties", "watts_per_meter_per_degree_kelvin", "W/(m·K)",
            new Dictionary<string, string>
            {
                { "newton_seconds", "value * 1" }, // Convert N·s to W/(m·K) (placeholder)
                { "newtons_per_meter", "value * 1" }, // Convert N/m to W/(m·K) (placeholder)
                { "pascal_seconds", "value * 1" }, // Convert Pa·s to W/(m·K) (placeholder)
                { "square_meters_per_newton", "value * 1" }, // Convert m²/N to W/(m·K) (placeholder)
                { "watts_per_square_meter_degree_kelvin", "value * 1" } // Convert W/(m²·K) to W/(m·K) (placeholder)
            }
        );

        public static IotUnit watts_per_square_meter_degree_kelvin => new IotUnit("PhysicalProperties", "watts_per_square_meter_degree_kelvin", "W/(m²·K)",
            new Dictionary<string, string>
            {
                { "newton_seconds", "value * 1" }, // Convert N·s to W/(m²·K) (placeholder)
                { "newtons_per_meter", "value * 1" }, // Convert N/m to W/(m²·K) (placeholder)
                { "pascal_seconds", "value * 1" }, // Convert Pa·s to W/(m²·K) (placeholder)
                { "square_meters_per_newton", "value * 1" }, // Convert m²/N to W/(m²·K) (placeholder)
                { "watts_per_meter_per_degree_kelvin", "value * 1" } // Convert W/(m·K) to W/(m²·K) (placeholder)
            }
        );

        // Power Units
        public static IotUnit horsepower => new IotUnit("Power", "horsepower", "hp",
            new Dictionary<string, string>
            {
                { "joule_per_hours", "value * 2684520" }, // Convert hp to J/h
                { "kilo_btus_per_hour", "value * 2.544" }, // Convert hp to kBTU/h
                { "kilowatts", "value * 0.735499" }, // Convert hp to kW
                { "megawatts", "value * 0.000735499" }, // Convert hp to MW
                { "milliwatts", "value * 735499" }, // Convert hp to mW
                { "tons_refrigeration", "value * 0.212121" }, // Convert hp to TR
                { "watts_power", "value * 735.499" }, // Convert hp to W
                { "btus_per_hour", "value * 2544.433" } // Convert hp to BTU/h
            }
        );

        public static IotUnit joule_per_hours => new IotUnit("Power", "joule_per_hours", "J/h",
            new Dictionary<string, string>
            {
                { "horsepower", "value / 2684520" }, // Convert J/h to hp
                { "kilo_btus_per_hour", "value / 1.055e6" }, // Convert J/h to kBTU/h
                { "kilowatts", "value / 3.6e6" }, // Convert J/h to kW
                { "megawatts", "value / 3.6e9" }, // Convert J/h to MW
                { "milliwatts", "value / 3.6" }, // Convert J/h to mW
                { "tons_refrigeration", "value / 1.055e6 * 0.212121" }, // Convert J/h to TR
                { "watts_power", "value / 3600" }, // Convert J/h to W
                { "btus_per_hour", "value / 1055" } // Convert J/h to BTU/h
            }
        );

        public static IotUnit kilo_btus_per_hour => new IotUnit("Power", "kilo_btus_per_hour", "kBTU/h",
            new Dictionary<string, string>
            {
                { "horsepower", "value / 2.544" }, // Convert kBTU/h to hp
                { "joule_per_hours", "value * 1.055e6" }, // Convert kBTU/h to J/h
                { "kilowatts", "value * 0.293071" }, // Convert kBTU/h to kW
                { "megawatts", "value * 0.000293071" }, // Convert kBTU/h to MW
                { "milliwatts", "value * 293071" }, // Convert kBTU/h to mW
                { "tons_refrigeration", "value * 0.0833333" }, // Convert kBTU/h to TR
                { "watts_power", "value * 293.071" }, // Convert kBTU/h to W
                { "btus_per_hour", "value * 1000" } // Convert kBTU/h to BTU/h
            }
        );

        public static IotUnit kilowatts => new IotUnit("Power", "kilowatts", "kW",
            new Dictionary<string, string>
            {
                { "horsepower", "value / 0.735499" }, // Convert kW to hp
                { "joule_per_hours", "value * 3.6e6" }, // Convert kW to J/h
                { "kilo_btus_per_hour", "value / 0.293071" }, // Convert kW to kBTU/h
                { "megawatts", "value / 1000" }, // Convert kW to MW
                { "milliwatts", "value * 1e6" }, // Convert kW to mW
                { "tons_refrigeration", "value * 0.284345" }, // Convert kW to TR
                { "watts_power", "value * 1000" }, // Convert kW to W
                { "btus_per_hour", "value * 3412.14" } // Convert kW to BTU/h
            }
        );

        public static IotUnit megawatts => new IotUnit("Power", "megawatts", "MW",
            new Dictionary<string, string>
            {
                { "horsepower", "value * 1340.481" }, // Convert MW to hp
                { "joule_per_hours", "value * 3.6e9" }, // Convert MW to J/h
                { "kilo_btus_per_hour", "value * 3412.14" }, // Convert MW to kBTU/h
                { "kilowatts", "value * 1000" }, // Convert MW to kW
                { "milliwatts", "value * 1e9" }, // Convert MW to mW
                { "tons_refrigeration", "value * 284.345" }, // Convert MW to TR
                { "watts_power", "value * 1e6" }, // Convert MW to W
                { "btus_per_hour", "value * 3.412e6" } // Convert MW to BTU/h
            }
        );

        public static IotUnit milliwatts => new IotUnit("Power", "milliwatts", "mW",
            new Dictionary<string, string>
            {
                { "horsepower", "value / 735499" }, // Convert mW to hp
                { "joule_per_hours", "value * 3.6" }, // Convert mW to J/h
                { "kilo_btus_per_hour", "value / 293071" }, // Convert mW to kBTU/h
                { "kilowatts", "value / 1e6" }, // Convert mW to kW
                { "megawatts", "value / 1e9" }, // Convert mW to MW
                { "tons_refrigeration", "value / 3.51685e6" }, // Convert mW to TR
                { "watts_power", "value / 1000" }, // Convert mW to W
                { "btus_per_hour", "value / 293.071" } // Convert mW to BTU/h
            }
        );

        public static IotUnit tons_refrigeration => new IotUnit("Power", "tons_refrigeration", "TR",
            new Dictionary<string, string>
            {
                { "horsepower", "value / 0.212121" }, // Convert TR to hp
                { "joule_per_hours", "value * 1.055e6 * 12" }, // Convert TR to J/h
                { "kilo_btus_per_hour", "value * 12" }, // Convert TR to kBTU/h
                { "kilowatts", "value / 0.284345" }, // Convert TR to kW
                { "megawatts", "value / 284.345" }, // Convert TR to MW
                { "milliwatts", "value * 3.51685e6" }, // Convert TR to mW
                { "watts_power", "value * 3516.85" }, // Convert TR to W
                { "btus_per_hour", "value * 12000" } // Convert TR to BTU/h
            }
        );

        public static IotUnit watts_power => new IotUnit("Power", "watts", "W",
            new Dictionary<string, string>
            {
                { "horsepower", "value / 735.499" }, // Convert W to hp
                { "joule_per_hours", "value * 3600" }, // Convert W to J/h
                { "kilo_btus_per_hour", "value / 293.071" }, // Convert W to kBTU/h
                { "kilowatts", "value / 1000" }, // Convert W to kW
                { "megawatts", "value / 1e6" }, // Convert W to MW
                { "milliwatts", "value * 1000" }, // Convert W to mW
                { "tons_refrigeration", "value / 3516.85" }, // Convert W to TR
                { "btus_per_hour", "value * 3.41214" } // Convert W to BTU/h
            }
        );

        public static IotUnit btus_per_hour => new IotUnit("Power", "btus_per_hour", "BTU/h",
            new Dictionary<string, string>
            {
                { "horsepower", "value / 2544.43" }, // Convert BTU/h to hp
                { "joule_per_hours", "value * 1055.06" }, // Convert BTU/h to J/h
                { "kilo_btus_per_hour", "value / 1000" }, // Convert BTU/h to kBTU/h
                { "kilowatts", "value / 3412.14" }, // Convert BTU/h to kW
                { "megawatts", "value / 3.412e6" }, // Convert BTU/h to MW
                { "milliwatts", "value * 293.071" }, // Convert BTU/h to mW
                { "tons_refrigeration", "value / 12000" }, // Convert BTU/h to TR
                { "watts_power", "value / 3.41214" } // Convert BTU/h to W
            }
        );


        // Pressure Units
        public static IotUnit bars_pressure => new IotUnit("Pressure", "bars", "bar",
            new Dictionary<string, string>
            {
        { "centimeters_of_mercury", "value / 75.0062" }, // Convert cmHg to bar
        { "centimeters_of_water", "value / 1019.72" }, // Convert cmH₂O to bar
        { "hectopascals", "value / 1000" }, // Convert hPa to bar
        { "inches_of_mercury", "value / 29.5301" }, // Convert inHg to bar
        { "inches_of_water", "value / 4014.74" }, // Convert inH₂O to bar
        { "kilopascals", "value / 100" }, // Convert kPa to bar
        { "millibars", "value / 1000" }, // Convert mbar to bar
        { "millimeters_of_mercury", "value / 750.062" }, // Convert mmHg to bar
        { "millimeters_of_water", "value / 101972" }, // Convert mmH₂O to bar
        { "pascals", "value / 100000" }, // Convert Pa to bar
        { "pounds_force_per_square_inch", "value / 14.5038" } // Convert psi to bar
            }
        );

        public static IotUnit centimeters_of_mercury => new IotUnit("Pressure", "centimeters_of_mercury", "cmHg",
            new Dictionary<string, string>
            {
        { "bars_pressure", "value * 75.0062" }, // Convert bar to cmHg
        { "centimeters_of_water", "value / 13.5951" }, // Convert cmH₂O to cmHg
        { "hectopascals", "value / 13.3322" }, // Convert hPa to cmHg
        { "inches_of_mercury", "value * 2.54" }, // Convert inHg to cmHg
        { "inches_of_water", "value / 5.33322" }, // Convert inH₂O to cmHg
        { "kilopascals", "value / 1.33322" }, // Convert kPa to cmHg
        { "millibars", "value / 13.3322" }, // Convert mbar to cmHg
        { "millimeters_of_mercury", "value / 10" }, // Convert mmHg to cmHg
        { "millimeters_of_water", "value / 135.951" }, // Convert mmH₂O to cmHg
        { "pascals", "value / 1333.22" }, // Convert Pa to cmHg
        { "pounds_force_per_square_inch", "value / 0.193367" } // Convert psi to cmHg
            }
        );

        public static IotUnit centimeters_of_water => new IotUnit("Pressure", "centimeters_of_water", "cmH₂O",
            new Dictionary<string, string>
            {
        { "bars_pressure", "value * 1019.72" }, // Convert bar to cmH₂O
        { "centimeters_of_mercury", "value * 13.5951" }, // Convert cmHg to cmH₂O
        { "hectopascals", "value / 0.980665" }, // Convert hPa to cmH₂O
        { "inches_of_mercury", "value * 34.5315" }, // Convert inHg to cmH₂O
        { "inches_of_water", "value * 2.54" }, // Convert inH₂O to cmH₂O
        { "kilopascals", "value * 101.972" }, // Convert kPa to cmH₂O
        { "millibars", "value / 0.980665" }, // Convert mbar to cmH₂O
        { "millimeters_of_mercury", "value * 13.5951" }, // Convert mmHg to cmH₂O
        { "millimeters_of_water", "value / 10" }, // Convert mmH₂O to cmH₂O
        { "pascals", "value / 98.0665" }, // Convert Pa to cmH₂O
        { "pounds_force_per_square_inch", "value / 0.0142233" } // Convert psi to cmH₂O
            }
        );

        public static IotUnit hectopascals => new IotUnit("Pressure", "hectopascals", "hPa",
            new Dictionary<string, string>
            {
        { "bars_pressure", "value * 1000" }, // Convert bar to hPa
        { "centimeters_of_mercury", "value * 13.3322" }, // Convert cmHg to hPa
        { "centimeters_of_water", "value * 0.980665" }, // Convert cmH₂O to hPa
        { "inches_of_mercury", "value * 33.8639" }, // Convert inHg to hPa
        { "inches_of_water", "value * 2.4884" }, // Convert inH₂O to hPa
        { "kilopascals", "value * 10" }, // Convert kPa to hPa
        { "millibars", "value * 1" }, // Convert mbar to hPa
        { "millimeters_of_mercury", "value * 1.33322" }, // Convert mmHg to hPa
        { "millimeters_of_water", "value / 10.1972" }, // Convert mmH₂O to hPa
        { "pascals", "value / 100" }, // Convert Pa to hPa
        { "pounds_force_per_square_inch", "value / 0.0145038" } // Convert psi to hPa
            }
        );

        public static IotUnit inches_of_mercury => new IotUnit("Pressure", "inches_of_mercury", "inHg",
            new Dictionary<string, string>
            {
        { "bars_pressure", "value * 29.5301" }, // Convert bar to inHg
        { "centimeters_of_mercury", "value / 2.54" }, // Convert cmHg to inHg
        { "centimeters_of_water", "value / 34.5315" }, // Convert cmH₂O to inHg
        { "hectopascals", "value / 33.8639" }, // Convert hPa to inHg
        { "inches_of_water", "value / 13.596" }, // Convert inH₂O to inHg
        { "kilopascals", "value / 3.38639" }, // Convert kPa to inHg
        { "millibars", "value / 33.8639" }, // Convert mbar to inHg
        { "millimeters_of_mercury", "value / 25.4" }, // Convert mmHg to inHg
        { "millimeters_of_water", "value / 345.315" }, // Convert mmH₂O to inHg
        { "pascals", "value / 3386.39" }, // Convert Pa to inHg
        { "pounds_force_per_square_inch", "value / 0.491154" } // Convert psi to inHg
            }
        );

        public static IotUnit kilopascals => new IotUnit("Pressure", "kilopascals", "kPa",
            new Dictionary<string, string>
            {
        { "bars_pressure", "value * 100" }, // Convert bar to kPa
        { "centimeters_of_mercury", "value * 1.33322" }, // Convert cmHg to kPa
        { "centimeters_of_water", "value / 101.972" }, // Convert cmH₂O to kPa
        { "hectopascals", "value / 10" }, // Convert hPa to kPa
        { "inches_of_mercury", "value * 3.38639" }, // Convert inHg to kPa
        { "inches_of_water", "value * 4.01474" }, // Convert inH₂O to kPa
        { "millibars", "value / 10" }, // Convert mbar to kPa
        { "millimeters_of_mercury", "value / 7.50062" }, // Convert mmHg to kPa
        { "millimeters_of_water", "value / 10197.2" }, // Convert mmH₂O to kPa
        { "pascals", "value / 1000" }, // Convert Pa to kPa
        { "pounds_force_per_square_inch", "value / 6.89476" } // Convert psi to kPa
            }
        );

        public static IotUnit pounds_force_per_square_inch => new IotUnit("Pressure", "pounds_force_per_square_inch", "psi",
    new Dictionary<string, string>
    {
        { "bars_pressure", "value * 14.5038" }, // Convert bar to psi
        { "centimeters_of_mercury", "value * 0.193367" }, // Convert cmHg to psi
        { "centimeters_of_water", "value * 0.0142233" }, // Convert cmH₂O to psi
        { "hectopascals", "value * 0.0145038" }, // Convert hPa to psi
        { "inches_of_mercury", "value * 0.491154" }, // Convert inHg to psi
        { "inches_of_water", "value * 0.0361273" }, // Convert inH₂O to psi
        { "kilopascals", "value / 6.89476" }, // Convert kPa to psi
        { "millibars", "value / 68.9476" }, // Convert mbar to psi
        { "millimeters_of_mercury", "value / 51.7149" }, // Convert mmHg to psi
        { "millimeters_of_water", "value / 703.069" }, // Convert mmH₂O to psi
        { "pascals", "value / 6894.76" } // Convert Pa to psi
    }
);

        public static IotUnit inches_of_water => new IotUnit("Pressure", "inches_of_water", "inH₂O",
            new Dictionary<string, string>
            {
        { "bars_pressure", "value * 4014.74" }, // Convert bar to inH₂O
        { "centimeters_of_mercury", "value * 5.33322" }, // Convert cmHg to inH₂O
        { "centimeters_of_water", "value / 2.54" }, // Convert cmH₂O to inH₂O
        { "hectopascals", "value / 2.4884" }, // Convert hPa to inH₂O
        { "inches_of_mercury", "value * 13.596" }, // Convert inHg to inH₂O
        { "kilopascals", "value / 0.249088" }, // Convert kPa to inH₂O
        { "millibars", "value / 2.4884" }, // Convert mbar to inH₂O
        { "millimeters_of_mercury", "value / 1.86832" }, // Convert mmHg to inH₂O
        { "millimeters_of_water", "value / 25.4" }, // Convert mmH₂O to inH₂O
        { "pascals", "value / 248.84" }, // Convert Pa to inH₂O
        { "pounds_force_per_square_inch", "value / 0.0361273" } // Convert psi to inH₂O
            }
        );

        public static IotUnit millibars => new IotUnit("Pressure", "millibars", "mbar",
            new Dictionary<string, string>
            {
        { "bars_pressure", "value * 1000" }, // Convert bar to mbar
        { "centimeters_of_mercury", "value * 13.3322" }, // Convert cmHg to mbar
        { "centimeters_of_water", "value / 0.980665" }, // Convert cmH₂O to mbar
        { "hectopascals", "value / 1" }, // Convert hPa to mbar
        { "inches_of_mercury", "value * 33.8639" }, // Convert inHg to mbar
        { "inches_of_water", "value * 2.4884" }, // Convert inH₂O to mbar
        { "kilopascals", "value * 10" }, // Convert kPa to mbar
        { "millimeters_of_mercury", "value * 1.33322" }, // Convert mmHg to mbar
        { "millimeters_of_water", "value / 10.1972" }, // Convert mmH₂O to mbar
        { "pascals", "value / 100" }, // Convert Pa to mbar
        { "pounds_force_per_square_inch", "value / 0.0145038" } // Convert psi to mbar
            }
        );

        public static IotUnit millimeters_of_mercury => new IotUnit("Pressure", "millimeters_of_mercury", "mmHg",
            new Dictionary<string, string>
            {
        { "bars_pressure", "value * 750.062" }, // Convert bar to mmHg
        { "centimeters_of_mercury", "value / 10" }, // Convert cmHg to mmHg
        { "centimeters_of_water", "value / 13.5951" }, // Convert cmH₂O to mmHg
        { "hectopascals", "value * 1.33322" }, // Convert hPa to mmHg
        { "inches_of_mercury", "value * 25.4" }, // Convert inHg to mmHg
        { "inches_of_water", "value * 1.86832" }, // Convert inH₂O to mmHg
        { "kilopascals", "value / 7.50062" }, // Convert kPa to mmHg
        { "millibars", "value / 1.33322" }, // Convert mbar to mmHg
        { "millimeters_of_water", "value / 135.951" }, // Convert mmH₂O to mmHg
        { "pascals", "value / 133.322" }, // Convert Pa to mmHg
        { "pounds_force_per_square_inch", "value / 51.7149" } // Convert psi to mmHg
            }
        );

        public static IotUnit millimeters_of_water => new IotUnit("Pressure", "millimeters_of_water", "mmH₂O",
            new Dictionary<string, string>
            {
        { "bars_pressure", "value * 101972" }, // Convert bar to mmH₂O
        { "centimeters_of_mercury", "value * 135.951" }, // Convert cmHg to mmH₂O
        { "centimeters_of_water", "value / 10" }, // Convert cmH₂O to mmH₂O
        { "hectopascals", "value / 10.1972" }, // Convert hPa to mmH₂O
        { "inches_of_mercury", "value * 345.315" }, // Convert inHg to mmH₂O
        { "inches_of_water", "value / 25.4" }, // Convert inH₂O to mmH₂O
        { "kilopascals", "value / 10197.2" }, // Convert kPa to mmH₂O
        { "millibars", "value * 10.1972" }, // Convert mbar to mmH₂O
        { "millimeters_of_mercury", "value * 135.951" }, // Convert mmHg to mmH₂O
        { "pascals", "value / 9.80665" }, // Convert Pa to mmH₂O
        { "pounds_force_per_square_inch", "value / 703.069" } // Convert psi to mmH₂O
            }
        );

        public static IotUnit pascals => new IotUnit("Pressure", "pascals", "Pa",
            new Dictionary<string, string>
            {
        { "bars_pressure", "value * 100000" }, // Convert bar to Pa
        { "centimeters_of_mercury", "value * 1333.22" }, // Convert cmHg to Pa
        { "centimeters_of_water", "value * 98.0665" }, // Convert cmH₂O to Pa
        { "hectopascals", "value * 100" }, // Convert hPa to Pa
        { "inches_of_mercury", "value * 3386.39" }, // Convert inHg to Pa
        { "inches_of_water", "value * 248.84" }, // Convert inH₂O to Pa
        { "kilopascals", "value * 1000" }, // Convert kPa to Pa
        { "millibars", "value * 100" }, // Convert mbar to Pa
        { "millimeters_of_mercury", "value * 133.322" }, // Convert mmHg to Pa
        { "millimeters_of_water", "value * 9.80665" }, // Convert mmH₂O to Pa
        { "pounds_force_per_square_inch", "value / 6894.76" } // Convert psi to Pa
            }
        );


        // Radiation Units
        public static IotUnit becquerels => new IotUnit("Radiation", "becquerels", "Bq",
            new Dictionary<string, string>
            {
        { "kilobecquerels", "value * 1000" }, // Convert kBq to Bq
        { "megabecquerels", "value * 1e6" }, // Convert MBq to Bq
        { "curies", "value * 3.7e10" } // Convert Ci to Bq
            }
        );

        public static IotUnit curies => new IotUnit("Radiation", "curies", "Ci",
            new Dictionary<string, string>
            {
        { "becquerels", "value / 3.7e10" }, // Convert Bq to Ci
        { "kilobecquerels", "value / 3.7e7" }, // Convert kBq to Ci
        { "megabecquerels", "value / 3.7e4" } // Convert MBq to Ci
            }
        );

        public static IotUnit gray => new IotUnit("Radiation", "gray", "Gy",
            new Dictionary<string, string>
            {
        { "milligray", "value / 1000" }, // Convert mGy to Gy
        { "microgray", "value / 1e6" }, // Convert µGy to Gy
        { "rads", "value / 100" } // Convert rad to Gy
            }
        );

        public static IotUnit kilobecquerels => new IotUnit("Radiation", "kilobecquerels", "kBq",
            new Dictionary<string, string>
            {
        { "becquerels", "value / 1000" }, // Convert Bq to kBq
        { "megabecquerels", "value * 1000" }, // Convert MBq to kBq
        { "curies", "value * 3.7e7" } // Convert Ci to kBq
            }
        );

        public static IotUnit megabecquerels => new IotUnit("Radiation", "megabecquerels", "MBq",
            new Dictionary<string, string>
            {
        { "becquerels", "value / 1e6" }, // Convert Bq to MBq
        { "kilobecquerels", "value / 1000" }, // Convert kBq to MBq
        { "curies", "value * 37" } // Convert Ci to MBq
            }
        );

        public static IotUnit milligray => new IotUnit("Radiation", "milligray", "mGy",
            new Dictionary<string, string>
            {
        { "gray", "value * 1000" }, // Convert Gy to mGy
        { "microgray", "value / 1000" }, // Convert µGy to mGy
        { "rads", "value * 10" } // Convert rad to mGy
            }
        );

        public static IotUnit millirems => new IotUnit("Radiation", "millirems", "mrem",
            new Dictionary<string, string>
            {
        { "rems", "value * 1000" }, // Convert rem to mrem
        { "millisieverts", "value * 100" }, // Convert mSv to mrem
        { "microsieverts", "value / 10" } // Convert µSv to mrem
            }
        );

        public static IotUnit millirems_per_hour => new IotUnit("Radiation", "millirems_per_hour", "mrem/h",
            new Dictionary<string, string>
            {
        { "microsieverts_per_hour", "value / 10" }, // Convert µSv/h to mrem/h
        { "millisieverts", "value * 100" }, // Convert mSv to mrem/h
        { "rems", "value * 1000" } // Convert rem to mrem/h
            }
        );

        public static IotUnit millisieverts => new IotUnit("Radiation", "millisieverts", "mSv",
            new Dictionary<string, string>
            {
        { "sieverts", "value * 1000" }, // Convert Sv to mSv
        { "microsieverts", "value / 1000" }, // Convert µSv to mSv
        { "millirems", "value / 100" } // Convert mrem to mSv
            }
        );

        public static IotUnit microsieverts => new IotUnit("Radiation", "microsieverts", "µSv",
            new Dictionary<string, string>
            {
        { "sieverts", "value * 1e6" }, // Convert Sv to µSv
        { "millisieverts", "value * 1000" }, // Convert mSv to µSv
        { "millirems", "value * 10" } // Convert mrem to µSv
            }
        );

        public static IotUnit microsieverts_per_hour => new IotUnit("Radiation", "microsieverts_per_hour", "µSv/h",
            new Dictionary<string, string>
            {
        { "millirems_per_hour", "value * 10" }, // Convert mrem/h to µSv/h
        { "millisieverts", "value * 1000" }, // Convert mSv to µSv/h
        { "rems", "value * 1e6" } // Convert rem to µSv/h
            }
        );

        public static IotUnit microgray => new IotUnit("Radiation", "microgray", "µGy",
            new Dictionary<string, string>
            {
        { "gray", "value * 1e6" }, // Convert Gy to µGy
        { "milligray", "value * 1000" }, // Convert mGy to µGy
        { "rads", "value * 1e5" } // Convert rad to µGy
            }
        );

        public static IotUnit rads => new IotUnit("Radiation", "rads", "rad",
            new Dictionary<string, string>
            {
        { "gray", "value * 100" }, // Convert Gy to rad
        { "milligray", "value / 10" }, // Convert mGy to rad
        { "microgray", "value / 1e5" } // Convert µGy to rad
            }
        );

        public static IotUnit rems => new IotUnit("Radiation", "rems", "rem",
            new Dictionary<string, string>
            {
        { "millirems", "value / 1000" }, // Convert mrem to rem
        { "millisieverts", "value / 10" }, // Convert mSv to rem
        { "microsieverts", "value / 1e4" } // Convert µSv to rem
            }
        );

        public static IotUnit sieverts => new IotUnit("Radiation", "sieverts", "Sv",
            new Dictionary<string, string>
            {
        { "millisieverts", "value / 1000" }, // Convert mSv to Sv
        { "microsieverts", "value / 1e6" }, // Convert µSv to Sv
        { "millirems", "value / 1e5" } // Convert mrem to Sv
            }
        );

        // Radiant Intensity Units
        public static IotUnit microwatts_per_steradian => new IotUnit("RadiantIntensity", "microwatts_per_steradian", "µW/sr",
            new Dictionary<string, string>
            {
        { "watts_per_steradian", "value * 1e6" } // Convert W/sr to µW/sr
            }
        );

        public static IotUnit watts_per_steradian => new IotUnit("RadiantIntensity", "watts_per_steradian", "W/sr",
            new Dictionary<string, string>
            {
        { "microwatts_per_steradian", "value / 1e6" } // Convert µW/sr to W/sr
            }
        );


        // Temperature Units
        public static IotUnit degree_days_celsius => new IotUnit("Temperature", "degree_days_celsius", "°C·d",
            new Dictionary<string, string>
            {
        { "degree_days_fahrenheit", "value / 1.8" } // Convert °F·d to °C·d
            }
        );

        public static IotUnit degree_days_fahrenheit => new IotUnit("Temperature", "degree_days_fahrenheit", "°F·d",
            new Dictionary<string, string>
            {
        { "degree_days_celsius", "value * 1.8" } // Convert °C·d to °F·d
            }
        );

        public static IotUnit degrees_celsius => new IotUnit("Temperature", "degrees_celsius", "°C",
            new Dictionary<string, string>
            {
        { "degrees_fahrenheit", "(value - 32) * 5 / 9" }, // Convert °F to °C
        { "degrees_kelvin", "value - 273.15" }, // Convert K to °C
        { "degrees_rankine", "(value - 491.67) * 5 / 9" }, // Convert °R to °C
        { "delta_degrees_fahrenheit", "value * 5 / 9" }, // Convert ∆°F to °C
        { "delta_degrees_kelvin", "value" } // Convert ∆K to °C
            }
        );

        public static IotUnit degrees_fahrenheit => new IotUnit("Temperature", "degrees_fahrenheit", "°F",
            new Dictionary<string, string>
            {
        { "degrees_celsius", "(value * 9 / 5) + 32" }, // Convert °C to °F
        { "degrees_kelvin", "(value - 273.15) * 9 / 5 + 32" }, // Convert K to °F
        { "degrees_rankine", "value + 459.67" }, // Convert °R to °F
        { "delta_degrees_fahrenheit", "value" }, // ∆°F to °F
        { "delta_degrees_kelvin", "(value * 9 / 5)" } // Convert ∆K to °F
            }
        );

        public static IotUnit degrees_kelvin => new IotUnit("Temperature", "degrees_kelvin", "K",
            new Dictionary<string, string>
            {
        { "degrees_celsius", "value + 273.15" }, // Convert °C to K
        { "degrees_fahrenheit", "(value - 32) * 5 / 9 + 273.15" }, // Convert °F to K
        { "degrees_rankine", "value * 5 / 9" }, // Convert °R to K
        { "delta_degrees_fahrenheit", "(value * 5 / 9) + 273.15" }, // Convert ∆°F to K
        { "delta_degrees_kelvin", "value + 273.15" } // Convert ∆K to K
            }
        );

        public static IotUnit degrees_rankine => new IotUnit("Temperature", "degrees_rankine", "°R",
            new Dictionary<string, string>
            {
        { "degrees_celsius", "(value * 9 / 5) + 491.67" }, // Convert °C to °R
        { "degrees_fahrenheit", "value - 459.67" }, // Convert °F to °R
        { "degrees_kelvin", "value * 9 / 5" }, // Convert K to °R
        { "delta_degrees_fahrenheit", "value * 9 / 5" }, // Convert ∆°F to °R
        { "delta_degrees_kelvin", "(value * 9 / 5)" } // Convert ∆K to °R
            }
        );

        public static IotUnit delta_degrees_fahrenheit => new IotUnit("Temperature", "delta_degrees_fahrenheit", "∆°F",
            new Dictionary<string, string>
            {
        { "degrees_celsius", "value * 9 / 5" }, // Convert °C to ∆°F
        { "degrees_kelvin", "(value - 273.15) * 9 / 5" }, // Convert K to ∆°F
        { "degrees_rankine", "value * 9 / 5" }, // Convert °R to ∆°F
        { "delta_degrees_kelvin", "value * 9 / 5" } // Convert ∆K to ∆°F
            }
        );

        public static IotUnit delta_degrees_kelvin => new IotUnit("Temperature", "delta_degrees_kelvin", "∆K",
            new Dictionary<string, string>
            {
        { "degrees_celsius", "value" }, // Convert °C to ∆K
        { "degrees_fahrenheit", "(value * 5 / 9)" }, // Convert °F to ∆K
        { "degrees_rankine", "(value * 5 / 9)" }, // Convert °R to ∆K
        { "delta_degrees_fahrenheit", "value * 5 / 9" } // Convert ∆°F to ∆K
            }
        );


        /// Temperature Rate Units
        public static IotUnit degrees_celsius_per_hour => new IotUnit("TemperatureRate", "degrees_celsius_per_hour", "°C/h",
            new Dictionary<string, string>
            {
        { "degrees_celsius_per_minute", "value * 60" }, // Convert °C/min to °C/h
        { "degrees_fahrenheit_per_hour", "(value * 9 / 5) + 32" }, // Convert °F/h to °C/h
        { "degrees_fahrenheit_per_minute", "((value * 9 / 5) + 32) * 60" }, // Convert °F/min to °C/h
        { "minutes_per_degree_kelvin", "1 / (value * 60)" }, // Convert min/K to °C/h
        { "psi_per_degree_fahrenheit", "value / ((value - 32) * 5 / 9)" } // Convert psi/°F to °C/h
            }
        );

        public static IotUnit degrees_celsius_per_minute => new IotUnit("TemperatureRate", "degrees_celsius_per_minute", "°C/min",
            new Dictionary<string, string>
            {
        { "degrees_celsius_per_hour", "value / 60" }, // Convert °C/h to °C/min
        { "degrees_fahrenheit_per_hour", "((value * 9 / 5) + 32) / 60" }, // Convert °F/h to °C/min
        { "degrees_fahrenheit_per_minute", "(value * 9 / 5) + 32" }, // Convert °F/min to °C/min
        { "minutes_per_degree_kelvin", "1 / value" }, // Convert min/K to °C/min
        { "psi_per_degree_fahrenheit", "value / ((value - 32) * 5 / 9)" } // Convert psi/°F to °C/min
            }
        );

        public static IotUnit degrees_fahrenheit_per_hour => new IotUnit("TemperatureRate", "degrees_fahrenheit_per_hour", "°F/h",
            new Dictionary<string, string>
            {
        { "degrees_celsius_per_hour", "(value - 32) * 5 / 9" }, // Convert °C/h to °F/h
        { "degrees_celsius_per_minute", "((value - 32) * 5 / 9) / 60" }, // Convert °C/min to °F/h
        { "degrees_fahrenheit_per_minute", "value / 60" }, // Convert °F/min to °F/h
        { "minutes_per_degree_kelvin", "1 / ((value - 32) * 5 / 9 * 60)" }, // Convert min/K to °F/h
        { "psi_per_degree_fahrenheit", "value" } // Convert psi/°F to °F/h
            }
        );

        public static IotUnit degrees_fahrenheit_per_minute => new IotUnit("TemperatureRate", "degrees_fahrenheit_per_minute", "°F/min",
            new Dictionary<string, string>
            {
        { "degrees_celsius_per_hour", "((value - 32) * 5 / 9) / 60" }, // Convert °C/h to °F/min
        { "degrees_celsius_per_minute", "(value - 32) * 5 / 9" }, // Convert °C/min to °F/min
        { "degrees_fahrenheit_per_hour", "value * 60" }, // Convert °F/h to °F/min
        { "minutes_per_degree_kelvin", "1 / ((value - 32) * 5 / 9)" }, // Convert min/K to °F/min
        { "psi_per_degree_fahrenheit", "value" } // Convert psi/°F to °F/min
            }
        );

        public static IotUnit minutes_per_degree_kelvin => new IotUnit("TemperatureRate", "minutes_per_degree_kelvin", "min/K",
            new Dictionary<string, string>
            {
        { "degrees_celsius_per_hour", "1 / (value * 60)" }, // Convert °C/h to min/K
        { "degrees_celsius_per_minute", "1 / value" }, // Convert °C/min to min/K
        { "degrees_fahrenheit_per_hour", "1 / (((value - 32) * 5 / 9) * 60)" }, // Convert °F/h to min/K
        { "degrees_fahrenheit_per_minute", "1 / ((value - 32) * 5 / 9)" }, // Convert °F/min to min/K
        { "psi_per_degree_fahrenheit", "1 / value" } // Convert psi/°F to min/K
            }
        );

        public static IotUnit psi_per_degree_fahrenheit => new IotUnit("TemperatureRate", "psi_per_degree_fahrenheit", "psi/°F",
            new Dictionary<string, string>
            {
        { "degrees_celsius_per_hour", "value * ((value - 32) * 5 / 9)" }, // Convert °C/h to psi/°F
        { "degrees_celsius_per_minute", "value * ((value - 32) * 5 / 9) / 60" }, // Convert °C/min to psi/°F
        { "degrees_fahrenheit_per_hour", "value" }, // Convert °F/h to psi/°F
        { "degrees_fahrenheit_per_minute", "value / 60" }, // Convert °F/min to psi/°F
        { "minutes_per_degree_kelvin", "value" } // Convert min/K to psi/°F
            }
        );


        // Time Units
        public static IotUnit days => new IotUnit("Time", "days", "d",
            new Dictionary<string, string>
            {
        { "hours", "value / 24" }, // Convert hours to days
        { "minutes", "value / 1440" }, // Convert minutes to days
        { "seconds", "value / 86400" }, // Convert seconds to days
        { "milliseconds", "value / 86400000" }, // Convert milliseconds to days
        { "hundredths_seconds", "value / 8640000000" }, // Convert hs to days
        { "weeks", "value * 7" }, // Convert weeks to days
        { "months", "value * 30" }, // Convert months to days
        { "years", "value * 365" } // Convert years to days
            }
        );

        public static IotUnit hours => new IotUnit("Time", "hours", "h",
            new Dictionary<string, string>
            {
        { "days", "value * 24" }, // Convert days to hours
        { "minutes", "value / 60" }, // Convert minutes to hours
        { "seconds", "value / 3600" }, // Convert seconds to hours
        { "milliseconds", "value / 3600000" }, // Convert milliseconds to hours
        { "hundredths_seconds", "value / 360000000" }, // Convert hs to hours
        { "weeks", "value * 168" }, // Convert weeks to hours
        { "months", "value * 720" }, // Convert months to hours
        { "years", "value * 8760" } // Convert years to hours
            }
        );

        public static IotUnit minutes => new IotUnit("Time", "minutes", "min",
            new Dictionary<string, string>
            {
        { "days", "value * 1440" }, // Convert days to minutes
        { "hours", "value * 60" }, // Convert hours to minutes
        { "seconds", "value / 60" }, // Convert seconds to minutes
        { "milliseconds", "value / 60000" }, // Convert milliseconds to minutes
        { "hundredths_seconds", "value / 6000000" }, // Convert hs to minutes
        { "weeks", "value * 10080" }, // Convert weeks to minutes
        { "months", "value * 43200" }, // Convert months to minutes
        { "years", "value * 525600" } // Convert years to minutes
            }
        );

        public static IotUnit seconds => new IotUnit("Time", "seconds", "s",
            new Dictionary<string, string>
            {
        { "days", "value * 86400" }, // Convert days to seconds
        { "hours", "value * 3600" }, // Convert hours to seconds
        { "minutes", "value * 60" }, // Convert minutes to seconds
        { "milliseconds", "value / 1000" }, // Convert milliseconds to seconds
        { "hundredths_seconds", "value / 100" }, // Convert hs to seconds
        { "weeks", "value * 604800" }, // Convert weeks to seconds
        { "months", "value * 2592000" }, // Convert months to seconds
        { "years", "value * 31536000" } // Convert years to seconds
            }
        );

        public static IotUnit milliseconds => new IotUnit("Time", "milliseconds", "ms",
            new Dictionary<string, string>
            {
        { "days", "value * 86400000" }, // Convert days to milliseconds
        { "hours", "value * 3600000" }, // Convert hours to milliseconds
        { "minutes", "value * 60000" }, // Convert minutes to milliseconds
        { "seconds", "value * 1000" }, // Convert seconds to milliseconds
        { "hundredths_seconds", "value / 10" }, // Convert hs to milliseconds
        { "weeks", "value * 604800000" }, // Convert weeks to milliseconds
        { "months", "value * 2592000000" }, // Convert months to milliseconds
        { "years", "value * 31536000000" } // Convert years to milliseconds
            }
        );

        public static IotUnit hundredths_seconds => new IotUnit("Time", "hundredths_seconds", "hs",
            new Dictionary<string, string>
            {
        { "days", "value * 8640000000" }, // Convert days to hs
        { "hours", "value * 360000000" }, // Convert hours to hs
        { "minutes", "value * 6000000" }, // Convert minutes to hs
        { "seconds", "value * 100" }, // Convert seconds to hs
        { "milliseconds", "value * 10" }, // Convert milliseconds to hs
        { "weeks", "value * 60480000000" }, // Convert weeks to hs
        { "months", "value * 259200000000" }, // Convert months to hs
        { "years", "value * 3153600000000" } // Convert years to hs
            }
        );

        public static IotUnit weeks => new IotUnit("Time", "weeks", "wk",
            new Dictionary<string, string>
            {
        { "days", "value / 7" }, // Convert days to weeks
        { "hours", "value / 168" }, // Convert hours to weeks
        { "minutes", "value / 10080" }, // Convert minutes to weeks
        { "seconds", "value / 604800" }, // Convert seconds to weeks
        { "milliseconds", "value / 604800000" }, // Convert milliseconds to weeks
        { "hundredths_seconds", "value / 60480000000" }, // Convert hs to weeks
        { "months", "value * 4.34524" }, // Convert months to weeks
        { "years", "value * 52.1429" } // Convert years to weeks
            }
        );

        public static IotUnit months => new IotUnit("Time", "months", "mo",
            new Dictionary<string, string>
            {
        { "days", "value / 30" }, // Convert days to months
        { "hours", "value / 720" }, // Convert hours to months
        { "minutes", "value / 43200" }, // Convert minutes to months
        { "seconds", "value / 2592000" }, // Convert seconds to months
        { "milliseconds", "value / 2592000000" }, // Convert milliseconds to months
        { "hundredths_seconds", "value / 259200000000" }, // Convert hs to months
        { "weeks", "value / 4.34524" }, // Convert weeks to months
        { "years", "value * 12" } // Convert years to months
            }
        );

        public static IotUnit years => new IotUnit("Time", "years", "yr",
            new Dictionary<string, string>
            {
        { "days", "value / 365" }, // Convert days to years
        { "hours", "value / 8760" }, // Convert hours to years
        { "minutes", "value / 525600" }, // Convert minutes to years
        { "seconds", "value / 31536000" }, // Convert seconds to years
        { "milliseconds", "value / 31536000000" }, // Convert milliseconds to years
        { "hundredths_seconds", "value / 3153600000000" }, // Convert hs to years
        { "weeks", "value / 52.1429" }, // Convert weeks to years
        { "months", "value / 12" } // Convert months to years
            }
        );


        // Torque Units
        public static IotUnit newton_meters => new IotUnit("Torque", "newton_meters", "N·m", null);

        // Velocity Units
        public static IotUnit feet_per_minute => new IotUnit("Velocity", "feet_per_minute", "ft/min",
            new Dictionary<string, string>
            {
        { "feet_per_second", "value / 60" }, // Convert ft/s to ft/min
        { "millimeters_per_second", "value * 5.08" }, // Convert mm/s to ft/min
        { "kilometers_per_hour", "value / 54.6807" }, // Convert km/h to ft/min
        { "meters_per_hour", "value / 3.28084" }, // Convert m/h to ft/min
        { "meters_per_minute", "value / 3.28084" }, // Convert m/min to ft/min
        { "meters_per_second", "value / 196.8504" }, // Convert m/s to ft/min
        { "miles_per_hour", "value / 88" }, // Convert mph to ft/min
        { "millimeters_per_minute", "value * 0.0508" } // Convert mm/min to ft/min
            }
        );

        public static IotUnit feet_per_second => new IotUnit("Velocity", "feet_per_second", "ft/s",
            new Dictionary<string, string>
            {
        { "feet_per_minute", "value * 60" }, // Convert ft/min to ft/s
        { "millimeters_per_second", "value * 304.8" }, // Convert mm/s to ft/s
        { "kilometers_per_hour", "value * 1.09728" }, // Convert km/h to ft/s
        { "meters_per_hour", "value * 0.3048" }, // Convert m/h to ft/s
        { "meters_per_minute", "value * 0.3048" }, // Convert m/min to ft/s
        { "meters_per_second", "value * 0.3048" }, // Convert m/s to ft/s
        { "miles_per_hour", "value * 1.46667" }, // Convert mph to ft/s
        { "millimeters_per_minute", "value * 18.288" } // Convert mm/min to ft/s
            }
        );

        public static IotUnit millimeters_per_second => new IotUnit("Velocity", "millimeters_per_second", "mm/s",
            new Dictionary<string, string>
            {
        { "feet_per_minute", "value / 5.08" }, // Convert ft/min to mm/s
        { "feet_per_second", "value / 304.8" }, // Convert ft/s to mm/s
        { "kilometers_per_hour", "value * 3.6" }, // Convert km/h to mm/s
        { "meters_per_hour", "value * 0.001" }, // Convert m/h to mm/s
        { "meters_per_minute", "value * 0.0166667" }, // Convert m/min to mm/s
        { "meters_per_second", "value * 0.001" }, // Convert m/s to mm/s
        { "miles_per_hour", "value * 0.000621371" }, // Convert mph to mm/s
        { "millimeters_per_minute", "value * 60" } // Convert mm/min to mm/s
            }
        );

        public static IotUnit kilometers_per_hour => new IotUnit("Velocity", "kilometers_per_hour", "km/h",
            new Dictionary<string, string>
            {
        { "feet_per_minute", "value * 54.6807" }, // Convert ft/min to km/h
        { "feet_per_second", "value * 0.911344" }, // Convert ft/s to km/h
        { "millimeters_per_second", "value / 3.6" }, // Convert mm/s to km/h
        { "meters_per_hour", "value * 1000" }, // Convert m/h to km/h
        { "meters_per_minute", "value * 16.6667" }, // Convert m/min to km/h
        { "meters_per_second", "value * 3.6" }, // Convert m/s to km/h
        { "miles_per_hour", "value * 1.60934" }, // Convert mph to km/h
        { "millimeters_per_minute", "value / 0.00006" } // Convert mm/min to km/h
            }
        );

        public static IotUnit meters_per_hour => new IotUnit("Velocity", "meters_per_hour", "m/h",
            new Dictionary<string, string>
            {
        { "feet_per_minute", "value * 3.28084" }, // Convert ft/min to m/h
        { "feet_per_second", "value * 0.0003048" }, // Convert ft/s to m/h
        { "millimeters_per_second", "value * 0.001" }, // Convert mm/s to m/h
        { "kilometers_per_hour", "value / 1000" }, // Convert km/h to m/h
        { "meters_per_minute", "value * 60" }, // Convert m/min to m/h
        { "meters_per_second", "value * 3600" }, // Convert m/s to m/h
        { "miles_per_hour", "value * 1.60934" }, // Convert mph to m/h
        { "millimeters_per_minute", "value * 0.0166667" } // Convert mm/min to m/h
            }
        );

        public static IotUnit meters_per_minute => new IotUnit("Velocity", "meters_per_minute", "m/min",
            new Dictionary<string, string>
            {
        { "feet_per_minute", "value * 3.28084" }, // Convert ft/min to m/min
        { "feet_per_second", "value * 0.0166667" }, // Convert ft/s to m/min
        { "millimeters_per_second", "value * 1000" }, // Convert mm/s to m/min
        { "kilometers_per_hour", "value / 60" }, // Convert km/h to m/min
        { "meters_per_hour", "value / 60" }, // Convert m/h to m/min
        { "meters_per_second", "value / 60" }, // Convert m/s to m/min
        { "miles_per_hour", "value * 26.8224" }, // Convert mph to m/min
        { "millimeters_per_minute", "value * 1000" } // Convert mm/min to m/min
            }
        );

        public static IotUnit meters_per_second => new IotUnit("Velocity", "meters_per_second", "m/s",
            new Dictionary<string, string>
            {
        { "feet_per_minute", "value * 196.8504" }, // Convert ft/min to m/s
        { "feet_per_second", "value * 3.28084" }, // Convert ft/s to m/s
        { "millimeters_per_second", "value / 1000" }, // Convert mm/s to m/s
        { "kilometers_per_hour", "value / 3.6" }, // Convert km/h to m/s
        { "meters_per_hour", "value / 3600" }, // Convert m/h to m/s
        { "meters_per_minute", "value / 60" }, // Convert m/min to m/s
        { "miles_per_hour", "value * 2.23694" }, // Convert mph to m/s
        { "millimeters_per_minute", "value / 60" } // Convert mm/min to m/s
            }
        );

        public static IotUnit miles_per_hour => new IotUnit("Velocity", "miles_per_hour", "mph",
            new Dictionary<string, string>
            {
        { "feet_per_minute", "value * 88" }, // Convert ft/min to mph
        { "feet_per_second", "value * 1.46667" }, // Convert ft/s to mph
        { "millimeters_per_second", "value / 0.000621371" }, // Convert mm/s to mph
        { "kilometers_per_hour", "value / 1.60934" }, // Convert km/h to mph
        { "meters_per_hour", "value / 1.60934" }, // Convert m/h to mph
        { "meters_per_minute", "value / 26.8224" }, // Convert m/min to mph
        { "meters_per_second", "value / 2.23694" }, // Convert m/s to mph
        { "millimeters_per_minute", "value / 0.000372822" } // Convert mm/min to mph
            }
        );

        public static IotUnit millimeters_per_minute => new IotUnit("Velocity", "millimeters_per_minute", "mm/min",
            new Dictionary<string, string>
            {
        { "feet_per_minute", "value / 0.0508" }, // Convert ft/min to mm/min
        { "feet_per_second", "value / 18.288" }, // Convert ft/s to mm/min
        { "millimeters_per_second", "value / 60" }, // Convert mm/s to mm/min
        { "kilometers_per_hour", "value * 0.00006" }, // Convert km/h to mm/min
        { "meters_per_hour", "value * 0.0166667" }, // Convert m/h to mm/min
        { "meters_per_minute", "value / 1000" }, // Convert m/min to mm/min
        { "meters_per_second", "value / 60000" }, // Convert m/s to mm/min
        { "miles_per_hour", "value * 0.000372822" } // Convert mph to mm/min
            }
        );


        // Volume Units
        public static IotUnit cubic_feet => new IotUnit("Volume", "cubic_feet", "ft³",
            new Dictionary<string, string>
            {
        { "cubic_meters", "value / 35.3147" }, // Convert m³ to ft³
        { "imperial_gallons", "value / 6.22884" }, // Convert gal (imp) to ft³
        { "liters", "value / 28.3168" }, // Convert L to ft³
        { "milliliters", "value / 28316.8" }, // Convert mL to ft³
        { "us_gallons", "value / 7.48052" } // Convert gal (US) to ft³
            }
        );

        public static IotUnit cubic_meters => new IotUnit("Volume", "cubic_meters", "m³",
            new Dictionary<string, string>
            {
        { "cubic_feet", "value * 35.3147" }, // Convert ft³ to m³
        { "imperial_gallons", "value * 219.969" }, // Convert gal (imp) to m³
        { "liters", "value * 1000" }, // Convert L to m³
        { "milliliters", "value * 1e6" }, // Convert mL to m³
        { "us_gallons", "value * 264.172" } // Convert gal (US) to m³
            }
        );

        public static IotUnit imperial_gallons => new IotUnit("Volume", "imperial_gallons", "gal (imp)",
            new Dictionary<string, string>
            {
        { "cubic_feet", "value * 6.22884" }, // Convert ft³ to gal (imp)
        { "cubic_meters", "value / 219.969" }, // Convert m³ to gal (imp)
        { "liters", "value * 4.54609" }, // Convert L to gal (imp)
        { "milliliters", "value * 4546.09" }, // Convert mL to gal (imp)
        { "us_gallons", "value * 1.20095" } // Convert gal (US) to gal (imp)
            }
        );

        public static IotUnit liters => new IotUnit("Volume", "liters", "L",
            new Dictionary<string, string>
            {
        { "cubic_feet", "value * 28.3168" }, // Convert ft³ to L
        { "cubic_meters", "value / 1000" }, // Convert m³ to L
        { "imperial_gallons", "value / 4.54609" }, // Convert gal (imp) to L
        { "milliliters", "value * 1000" }, // Convert mL to L
        { "us_gallons", "value / 3.78541" } // Convert gal (US) to L
            }
        );

        public static IotUnit milliliters => new IotUnit("Volume", "milliliters", "mL",
            new Dictionary<string, string>
            {
        { "cubic_feet", "value * 28316.8" }, // Convert ft³ to mL
        { "cubic_meters", "value / 1e6" }, // Convert m³ to mL
        { "imperial_gallons", "value / 4546.09" }, // Convert gal (imp) to mL
        { "liters", "value / 1000" }, // Convert L to mL
        { "us_gallons", "value / 3785.41" } // Convert gal (US) to mL
            }
        );

        public static IotUnit us_gallons => new IotUnit("Volume", "us_gallons", "gal (US)",
            new Dictionary<string, string>
            {
        { "cubic_feet", "value * 7.48052" }, // Convert ft³ to gal (US)
        { "cubic_meters", "value / 264.172" }, // Convert m³ to gal (US)
        { "imperial_gallons", "value / 1.20095" }, // Convert gal (imp) to gal (US)
        { "liters", "value * 3.78541" }, // Convert L to gal (US)
        { "milliliters", "value * 3785.41" } // Convert mL to gal (US)
            }
        );


        // Volume Specific Units
        public static IotUnit cubic_feet_per_pound => new IotUnit("VolumeSpecific", "cubic_feet_per_pound", "ft³/lb",
            new Dictionary<string, string>
            {
        { "cubic_meters_per_kilogram", "value / 16.0185" } // Convert m³/kg to ft³/lb
            }
        );

        public static IotUnit cubic_meters_per_kilogram => new IotUnit("VolumeSpecific", "cubic_meters_per_kilogram", "m³/kg",
            new Dictionary<string, string>
            {
        { "cubic_feet_per_pound", "value * 16.0185" } // Convert ft³/lb to m³/kg
            }
        );


        // Volumetric Flow Units
        public static IotUnit cubic_feet_per_day => new IotUnit("VolumetricFlow", "cubic_feet_per_day", "ft³/d",
            new Dictionary<string, string>
            {
        { "cubic_feet_per_hour", "value / 24" }, // Convert ft³/h to ft³/d
        { "cubic_feet_per_minute", "value / 1440" }, // Convert cfm to ft³/d
        { "cubic_feet_per_second", "value / 86400" }, // Convert cfs to ft³/d
        { "cubic_meters_per_day", "value / 35.3147" }, // Convert m³/d to ft³/d
        { "imperial_gallons_per_minute", "value / 8998.52" }, // Convert gal (imp)/min to ft³/d
        { "liters_per_hour", "value / 101.94" }, // Convert L/h to ft³/d
        { "us_gallons_per_minute", "value / 7480" } // Convert gpm to ft³/d
            }
        );

        public static IotUnit cubic_feet_per_hour => new IotUnit("VolumetricFlow", "cubic_feet_per_hour", "ft³/h",
            new Dictionary<string, string>
            {
        { "cubic_feet_per_day", "value * 24" }, // Convert ft³/d to ft³/h
        { "cubic_feet_per_minute", "value / 60" }, // Convert cfm to ft³/h
        { "cubic_feet_per_second", "value / 3600" }, // Convert cfs to ft³/h
        { "cubic_meters_per_hour", "value / 35.3147" }, // Convert m³/h to ft³/h
        { "imperial_gallons_per_minute", "value / 599.88" }, // Convert gal (imp)/min to ft³/h
        { "liters_per_hour", "value * 28.3168" }, // Convert L/h to ft³/h
        { "us_gallons_per_minute", "value / 8.0208" } // Convert gpm to ft³/h
            }
        );

        public static IotUnit cubic_feet_per_minute => new IotUnit("VolumetricFlow", "cubic_feet_per_minute", "cfm",
            new Dictionary<string, string>
            {
        { "cubic_feet_per_day", "value * 1440" }, // Convert ft³/d to cfm
        { "cubic_feet_per_hour", "value * 60" }, // Convert ft³/h to cfm
        { "cubic_feet_per_second", "value / 60" }, // Convert cfs to cfm
        { "cubic_meters_per_minute", "value / 35.3147" }, // Convert m³/min to cfm
        { "imperial_gallons_per_minute", "value * 6.22884" }, // Convert gal (imp)/min to cfm
        { "liters_per_minute", "value * 28.3168" }, // Convert L/min to cfm
        { "us_gallons_per_minute", "value * 7.48052" } // Convert gpm to cfm
            }
        );

        public static IotUnit cubic_feet_per_second => new IotUnit("VolumetricFlow", "cubic_feet_per_second", "cfs",
            new Dictionary<string, string>
            {
        { "cubic_feet_per_day", "value * 86400" }, // Convert ft³/d to cfs
        { "cubic_feet_per_hour", "value * 3600" }, // Convert ft³/h to cfs
        { "cubic_feet_per_minute", "value * 60" }, // Convert cfm to cfs
        { "cubic_meters_per_second", "value / 35.3147" }, // Convert m³/s to cfs
        { "imperial_gallons_per_minute", "value * 448.831" }, // Convert gal (imp)/min to cfs
        { "liters_per_second", "value * 28.3168" }, // Convert L/s to cfs
        { "us_gallons_per_minute", "value * 448.831" } // Convert gpm to cfs
            }
        );

        public static IotUnit cubic_meters_per_day => new IotUnit("VolumetricFlow", "cubic_meters_per_day", "m³/d",
            new Dictionary<string, string>
            {
        { "cubic_feet_per_day", "value * 35.3147" }, // Convert ft³/d to m³/d
        { "cubic_meters_per_hour", "value / 24" }, // Convert m³/h to m³/d
        { "liters_per_hour", "value * 41.6667" }, // Convert L/h to m³/d
        { "us_gallons_per_hour", "value * 264.172" }, // Convert gal (US)/h to m³/d
        { "us_gallons_per_minute", "value * 15850.3" } // Convert gpm to m³/d
            }
        );

        public static IotUnit liters_per_hour => new IotUnit("VolumetricFlow", "liters_per_hour", "L/h",
            new Dictionary<string, string>
            {
        { "cubic_feet_per_hour", "value / 28.3168" }, // Convert ft³/h to L/h
        { "cubic_meters_per_hour", "value / 1000" }, // Convert m³/h to L/h
        { "liters_per_minute", "value / 60" }, // Convert L/min to L/h
        { "us_gallons_per_hour", "value / 3.78541" }, // Convert gal (US)/h to L/h
        { "us_gallons_per_minute", "value / 0.0630902" } // Convert gpm to L/h
            }
        );

        public static IotUnit liters_per_minute => new IotUnit("VolumetricFlow", "liters_per_minute", "L/min",
            new Dictionary<string, string>
            {
        { "cubic_feet_per_minute", "value / 28.3168" }, // Convert cfm to L/min
        { "cubic_meters_per_minute", "value / 1000" }, // Convert m³/min to L/min
        { "liters_per_hour", "value * 60" }, // Convert L/h to L/min
        { "us_gallons_per_minute", "value / 3.78541" } // Convert gpm to L/min
            }
        );

        public static IotUnit us_gallons_per_minute => new IotUnit("VolumetricFlow", "us_gallons_per_minute", "gpm",
            new Dictionary<string, string>
            {
        { "cubic_feet_per_minute", "value / 7.48052" }, // Convert cfm to gpm
        { "cubic_meters_per_minute", "value / 15850.3" }, // Convert m³/min to gpm
        { "liters_per_minute", "value * 3.78541" }, // Convert L/min to gpm
        { "us_gallons_per_hour", "value * 60" } // Convert gal (US)/h to gpm
            }
        );

        public static IotUnit cubic_meters_per_hour => new IotUnit("VolumetricFlow", "cubic_meters_per_hour", "m³/h",
    new Dictionary<string, string>
    {
        { "cubic_feet_per_hour", "value * 35.3147" }, // Convert ft³/h to m³/h
        { "cubic_meters_per_day", "value * 24" }, // Convert m³/d to m³/h
        { "liters_per_hour", "value * 1000" }, // Convert L/h to m³/h
        { "us_gallons_per_hour", "value * 264.172" }, // Convert gal (US)/h to m³/h
        { "us_gallons_per_minute", "value * 15.8503" } // Convert gpm to m³/h
    }
);

        public static IotUnit cubic_meters_per_minute => new IotUnit("VolumetricFlow", "cubic_meters_per_minute", "m³/min",
            new Dictionary<string, string>
            {
        { "cubic_feet_per_minute", "value * 35.3147" }, // Convert cfm to m³/min
        { "cubic_meters_per_hour", "value * 60" }, // Convert m³/h to m³/min
        { "liters_per_minute", "value * 1000" }, // Convert L/min to m³/min
        { "us_gallons_per_minute", "value * 3.78541" } // Convert gpm to m³/min
            }
        );

        public static IotUnit cubic_meters_per_second => new IotUnit("VolumetricFlow", "cubic_meters_per_second", "m³/s",
            new Dictionary<string, string>
            {
        { "cubic_feet_per_second", "value * 35.3147" }, // Convert cfs to m³/s
        { "cubic_meters_per_minute", "value * 60" }, // Convert m³/min to m³/s
        { "liters_per_second", "value * 1000" }, // Convert L/s to m³/s
        { "us_gallons_per_minute", "value * 0.0630902" } // Convert gpm to m³/s
            }
        );

        public static IotUnit imperial_gallons_per_minute => new IotUnit("VolumetricFlow", "imperial_gallons_per_minute", "gal (imp)/min",
            new Dictionary<string, string>
            {
        { "cubic_feet_per_minute", "value / 6.22884" }, // Convert cfm to gal (imp)/min
        { "cubic_meters_per_minute", "value / 0.219969" }, // Convert m³/min to gal (imp)/min
        { "liters_per_minute", "value / 4.54609" }, // Convert L/min to gal (imp)/min
        { "us_gallons_per_minute", "value / 1.20095" } // Convert gpm to gal (imp)/min
            }
        );

        public static IotUnit milliliters_per_second => new IotUnit("VolumetricFlow", "milliliters_per_second", "mL/s",
            new Dictionary<string, string>
            {
        { "liters_per_second", "value / 1000" }, // Convert L/s to mL/s
        { "cubic_meters_per_second", "value / 1e6" }, // Convert m³/s to mL/s
        { "cubic_feet_per_second", "value / 28316.8" }, // Convert cfs to mL/s
        { "us_gallons_per_minute", "value / 0.0630902" } // Convert gpm to mL/s
            }
        );

        public static IotUnit million_standard_cubic_feet_per_day => new IotUnit("VolumetricFlow", "million_standard_cubic_feet_per_day", "MMscfd",
            new Dictionary<string, string>
            {
        { "cubic_feet_per_day", "value * 1e6" }, // Convert ft³/d to MMscfd
        { "thousand_standard_cubic_feet_per_day", "value * 1000" }, // Convert Mscfd to MMscfd
        { "cubic_meters_per_day", "value * 28316.8" }, // Convert m³/d to MMscfd
        { "us_gallons_per_hour", "value * 7480" } // Convert gal (US)/h to MMscfd
            }
        );

        public static IotUnit thousand_cubic_feet_per_day => new IotUnit("VolumetricFlow", "thousand_cubic_feet_per_day", "Mscfd",
    new Dictionary<string, string>
    {
        { "cubic_feet_per_day", "value * 1000" }, // Convert ft³/d to Mscfd
        { "million_standard_cubic_feet_per_day", "value / 1000" }, // Convert MMscfd to Mscfd
        { "cubic_meters_per_day", "value * 28.3168" }, // Convert m³/d to Mscfd
        { "us_gallons_per_hour", "value * 748" } // Convert gal (US)/h to Mscfd
    }
);

        public static IotUnit thousand_standard_cubic_feet_per_day => new IotUnit("VolumetricFlow", "thousand_standard_cubic_feet_per_day", "Mscfd",
            new Dictionary<string, string>
            {
        { "cubic_feet_per_day", "value * 1000" }, // Convert ft³/d to Mscfd
        { "million_standard_cubic_feet_per_day", "value / 1000" }, // Convert MMscfd to Mscfd
        { "cubic_meters_per_day", "value * 28.3168" }, // Convert m³/d to Mscfd
        { "us_gallons_per_hour", "value * 748" } // Convert gal (US)/h to Mscfd
            }
        );

        public static IotUnit pounds_mass_per_day => new IotUnit("VolumetricFlow", "pounds_mass_per_day", "lb/d",
            new Dictionary<string, string>
            {
        { "cubic_feet_per_day", "value / 62.4" }, // Convert ft³/d to lb/d
        { "cubic_meters_per_day", "value * 2204.62" }, // Convert m³/d to lb/d
        { "liters_per_hour", "value * 2.20462" }, // Convert L/h to lb/d
        { "us_gallons_per_hour", "value * 8.34" } // Convert gal (US)/h to lb/d
            }
        );

        public static IotUnit us_gallons_per_hour => new IotUnit("VolumetricFlow", "us_gallons_per_hour", "gal (US)/h",
            new Dictionary<string, string>
            {
        { "cubic_feet_per_hour", "value / 7.48052" }, // Convert ft³/h to gal (US)/h
        { "cubic_meters_per_hour", "value / 0.264172" }, // Convert m³/h to gal (US)/h
        { "liters_per_hour", "value / 3.78541" }, // Convert L/h to gal (US)/h
        { "us_gallons_per_minute", "value * 60" } // Convert gpm to gal (US)/h
            }
        );

        public static IotUnit standard_cubic_feet_per_day => new IotUnit("VolumetricFlow", "standard_cubic_feet_per_day", "scfd",
    new Dictionary<string, string>
    {
        { "cubic_feet_per_day", "value * 1" }, // Convert ft³/d to scfd
        { "thousand_cubic_feet_per_day", "value / 1000" }, // Convert Mscfd to scfd
        { "million_standard_cubic_feet_per_day", "value / 1e6" }, // Convert MMscfd to scfd
        { "cubic_meters_per_day", "value * 35.3147" }, // Convert m³/d to scfd
        { "us_gallons_per_hour", "value * 7.48052" } // Convert gal (US)/h to scfd
    }
);

        public static IotUnit million_standard_cubic_feet_per_minute => new IotUnit("VolumetricFlow", "million_standard_cubic_feet_per_minute", "MMscfm",
            new Dictionary<string, string>
            {
        { "cubic_feet_per_minute", "value * 1e6" }, // Convert cfm to MMscfm
        { "cubic_meters_per_minute", "value * 28316.8" }, // Convert m³/min to MMscfm
        { "liters_per_minute", "value * 28316800" }, // Convert L/min to MMscfm
        { "us_gallons_per_minute", "value * 7.48052" } // Convert gpm to MMscfm
            }
        );

        public static IotUnit liters_per_second => new IotUnit("VolumetricFlow", "liters_per_second", "L/s",
    new Dictionary<string, string>
    {
        { "cubic_feet_per_second", "value / 28.3168" }, // Convert cfs to L/s
        { "cubic_meters_per_second", "value * 1000" }, // Convert m³/s to L/s
        { "liters_per_minute", "value * 60" }, // Convert L/min to L/s
        { "milliliters_per_second", "value / 1000" }, // Convert mL/s to L/s
        { "us_gallons_per_minute", "value * 3.78541" } // Convert gpm to L/s
    }
);



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

