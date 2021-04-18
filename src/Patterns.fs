module Patterns

open System.Text.RegularExpressions

let replace (pattern: string, value: string) (input: string) =
    Regex.Replace(input, pattern, value, RegexOptions.Multiline ||| RegexOptions.ECMAScript)

let replaceWhile (pattern: string, value: string) (input: string) =
    let mutable res = input
    while Regex.IsMatch(res, pattern) do
        res <- Regex.Replace(res, pattern, value, RegexOptions.Multiline ||| RegexOptions.ECMAScript)
    res

let replacePatterns text =
    text
    |> replace (@" +$", @"")                                        // remove trailing spaces
    |> replace (@"^use ", @"//open ")                               // replace "use " with "open"
    |> replace (@"^(#\[|#!\[)", @"//$1")                            // comment "#[" and "#!["

    |> replace (@"^(pub |crate )?mod (.+);$", @"open $2")           // replace mod with open
    |> replace (@"^( {4,})(pub )?(\w+ *:[^:].*),\s*\}$", @"$1$3 }") // replace struct fields 1
    |> replace (@"^( {1,4})(pub )?(\w+ *:[^:].*),$", @"$1$3")       // replace struct fields 2
    |> replace (@"^(pub )?struct (.+)\{$", @"type $2= {")           // replace struct with type
    |> replace (@"^( {1,4})(\w+)\(([^)\n]+)\),( *//.*)?$", @"$1| $2 of $3") // replace union cases 1
    |> replace (@"^( {1,4})(\w+) \{(.*)\},( *//.*)?$", @"$1| $2 of $3")     // replace union cases 2
    |> replace (@"^( {1,4})(\w+),( *//.*)?$", @"$1| $2")            // replace union cases 3
    |> replace (@"^( {1,4})(\w+) \{$", @"$1| $2 of")                // replace union cases 4
    |> replace (@"^(pub |crate )?enum (.+)\{$", @"type $2=")        // replace enum with type

    |> replace (@"^(pub |crate )?struct ([^(]+)\(([^)]+)\);",
                @"type $2 = $3")                                    // replace struct with type alias
    |> replace (@"^(pub |crate )?(struct|enum) (.*)?;$",
                @"type $3")                                         // replace struct/enum with type

    |> replace (@"^( +)(pub |crate )?fn ([^(]+)\(\s*&(mut )?self,?\s*(.+)\{$",
                @"$1member self.$3($5=")                            // replace fn with member 1
    |> replace (@"^( +)(pub |crate )?fn ([^(]+)\(\s*&(mut )?self,?\s*",
                @"$1member self.$3(")                               // replace fn with member 2
    |> replace (@"^( +)(pub |crate )?fn new\b",
                @"$1static member new_")                            // replace fn with member 3

    |> replace (@"^( *)(pub |crate )?fn (.+)\{$", @"$1let $3=")     // replace fn with let 1
    |> replace (@"^( *)(pub |crate )?fn (.+)$", @"$1let $3")        // replace fn with let 2
    |> replace (@"^( *)(pub |crate )?const (.+)$", @"$1let $3")     // replace const with let

    |> replace (@"^(pub |crate )?impl\S* (.*) for (.*) \{",
                @"interface $2 with // for $3")                     // replace impl with interface

    |> replace (@"^(pub |crate )?impl\S* (.*)\{", @"type $2with")   // replace impl with "type with"
    |> replace (@"^(pub |crate )?trait\S* (.*)\{", @"type $2with")  // replace trait with "type with"
    |> replace (@"\bpub ", @"")                                     // replace "pub " with ""
    |> replace (@"\bcrate ", @"")                                   // replace "crate " with ""

    |> replace (@"^(\s*)where(\s*[^{]+),\s*\{", @"$1when$2 =")      // replace where's "{" with "when"
    |> replace (@"(\bif .*)\{", @"$1then")                          // replace if's "{" with "then"
    |> replace (@"(\}\s*)?(else)\s*\{", @"$2")                      // replace "} else {" with "else"
    |> replace (@"(\bmatch .*)\{", @"$1with")                       // replace match's "{" with "with"
    |> replace (@"(\bfor .*)\{", @"$1do")                           // replace for's "{" with "do"
    |> replace (@"(\while .*)\{", @"$1do")                          // replace while's "{" with "do"
    |> replace (@"\bref ", @"")                                     // replace "ref " with ""
    |> replace (@"\blet mut ", @"let mutable ")                     // replace "mut" with "mutable"
    |> replace (@"\bmut ", @"")                                     // replace "mut" with ""

    |> replace (@"(->.*)\{", @"$1=")                                // replace "-> * {" with "-> * ="
    |> replace (@"\)[^\S]*->[^\S]*", @"): ")                        // replace ") -> " with "): "
    |> replace (@"\|(.*?)\|\s+\{", @"fun ($1) ->")                  // replace closures with fun

    |> replace (@"^\s*\}[,;]?\n", @"")                              // replace "}[,;]" with ""
    |> replace (@"^( +)(.+?=>)", @"$1| $2")                         // replace "*=>" with "| *=>"
    |> replace (@"=>( \{)?( )?([^,\n]*),?$", @"->$2$3")             // replace "=>" with "->"
    |> replace (@"::", @".")                                        // replace "::" with "."

    |> replace (@"([^\w])!([^\s=;,]+)", @"$1not($2)")               // replace "!" with "not"
    |> replace (@"!\(", @"(")                                       // replace "!(" with "("
    |> replace (@"\)\?", @")")                                      // replace ")?" with ")"
    |> replace (@"&", @"")                                          // replace "&" with ""
    |> replace (@";$", @"")                                         // replace ";" with ""
    |> replace (@"([^/])[*]([^/])", @"$1$2")                        // replace "*" with ""
    |> replace (@"[/][*]", @"(*")                                   // replace "/*" with "(*"
    |> replace (@"[*][/]", @"*)")                                   // replace "*/" with "*)"

    |> replace (@"^( {8,}\w+): (.*[^=,]),?$", @"$1 = $2")           // replace ":" with "=" in records
    |> replace (@"\.to_string\(\)", @"")                            // replace ".to_string()" with ""
    |> replace (@"vec!\[(.*?)\]", @"Vec($1)")                       // replace "vec![]" with "Vec()"
    |> replace (@"\): Self\b", @")")                                // replace "): Self" with ")"
