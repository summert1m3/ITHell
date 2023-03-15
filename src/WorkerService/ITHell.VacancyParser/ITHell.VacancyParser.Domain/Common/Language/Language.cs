namespace ITHell.VacancyParser.Domain.Common.Language;

public enum Language
{
    Unknown,
    [MultipleDescription("Russian", "Русский")]
    Russian,
    [MultipleDescription("English", "Английский")]
    English,
    [MultipleDescription("Kazakh", "Казахский")]
    Kazakh,
    [MultipleDescription("Belarusian", "Белорусский")]
    Belarusian,
    [MultipleDescription("German", "Немецкий")]
    German,
    [MultipleDescription("French", "Французский")]
    French,
    [MultipleDescription("Italian", "Итальянский")]
    Italian,
    [MultipleDescription("Spanish", "Испанский")]
    Spanish,
    [MultipleDescription("Portuguese", "Португальский")]
    Portuguese,
    [MultipleDescription("Polish", "Польский")]
    Polish,
    [MultipleDescription("Ukrainian", "Украинский")]
    Ukrainian,
    [MultipleDescription("Czech", "Чешский")]
    Czech,
    [MultipleDescription("Slovak", "Словацкий")]
    Slovak,
    [MultipleDescription("Bulgarian", "Болгарский")]
    Bulgarian,
    [MultipleDescription("Hungarian", "Венгерский")]
    Hungarian,
    [MultipleDescription("Romanian", "Румынский")]
    Romanian,
    [MultipleDescription("Serbian", "Сербский")]
    Serbian,
    [MultipleDescription("Croatian", "Хорватский")]
    Croatian,
    [MultipleDescription("Slovenian", "Словенский")]
    Slovenian,
    [MultipleDescription("Macedonian", "Македонский")]
    Macedonian,
    [MultipleDescription("Albanian", "Албанский")]
    Albanian,
    [MultipleDescription("Kyrgyz", "Кыргызский")]
    Kyrgyz,
    [MultipleDescription("Uzbek", "Узбекский")]
    Uzbek,
    [MultipleDescription("Turkmen", "Туркменский")]
    Turkmen,
    [MultipleDescription("Azerbaijanian", "Азербайджанский")]
    Azerbaijanian
}