using ITHell.VacancyParser.Domain.Common.Attributes;

namespace ITHell.VacancyParser.Domain.Common;

public enum ProgrammingLanguage
{
    [MultipleDescription("C#")]
    CSharp,
    [MultipleDescription("Dart")]
    Dart,
    [MultipleDescription("Java")]
    Java,
    [MultipleDescription("C")]
    C,
    [MultipleDescription("Python")]
    Python,
    [MultipleDescription("C++")]
    CPP,
    [MultipleDescription("JavaScript", "JS")]
    JavaScript,
    [MultipleDescription("PHP")]
    PHP,
    [MultipleDescription("ObjectiveC")]
    ObjectiveC,
    [MultipleDescription("Assembly")]
    Assembly,
    [MultipleDescription("Ruby")]
    Ruby,
    [MultipleDescription("Swift")]
    Swift,
    [MultipleDescription("Kotlin")]
    Kotlin,
    [MultipleDescription("Go")]
    Go,
    [MultipleDescription("Perl")]
    Perl,
    [MultipleDescription("TypeScript", "TS")]
    TypeScript,
    [MultipleDescription("Lua")]
    Lua,
    [MultipleDescription("Scala")]
    Scala,
    [MultipleDescription("Rust")]
    Rust,
    [MultipleDescription("F#")]
    FSharp,
    [MultipleDescription("Elixir")]
    Elixir,
    [MultipleDescription("Delphi")]
    Delphi,
    [MultipleDescription("Erlang")]
    Erlang
}