using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using EasyAssertions;
using OctoParamsCounter;

namespace Tests
{
    [TestClass]
    public class OctoParamsCounterTests
    {
        [TestMethod]
        public void ExtractOctoParams_BasicParameters()
        {
            Regex search = new Regex(Options.DefaultPattern, RegexOptions.Compiled);

            Program.ExtractOctoParams(search, "$OctopusParameters['Foo']").ShouldOnlyContain(new[] { "Foo" });
            Program.ExtractOctoParams(search, "$OctopusParameters[\"Foo\"]").ShouldOnlyContain(new[] { "Foo" });
            Program.ExtractOctoParams(search, "#{Foo}").ShouldOnlyContain(new[] { "Foo" });
            Program.ExtractOctoParams(search, "#{Foo}, #{Bar}").ShouldOnlyContain(new[] { "Foo", "Bar" });
        }

        [TestMethod]
        public void ExtractOctoParams_ParamsWithFilters()
        {
            Regex search = new Regex(Options.DefaultPattern, RegexOptions.Compiled);

            Program.ExtractOctoParams(search, "#{Foo | XmlEscape}").ShouldOnlyContain(new[] { "Foo" });
            Program.ExtractOctoParams(search, "$OctopusParameters['Foo | XmlEscape']").ShouldOnlyContain(new[] { "Foo" });
        }

        [TestMethod]
        public void ExtractOctoParams_ParamsWithConditional()
        {
            Regex search = new Regex(Options.DefaultPattern, RegexOptions.Compiled);

            Program.ExtractOctoParams(search, "#{if Foo}").ShouldOnlyContain(new[] { "Foo" });
            Program.ExtractOctoParams(search, "#{IF Foo}").ShouldOnlyContain(new[] { "Foo" });
            Program.ExtractOctoParams(search, "#{/if}").ShouldBeEmpty();
        }

        [TestMethod]
        public void ExtractOctoParams_ParamsFromOutpu()
        {
            Regex search = new Regex(Options.DefaultPattern, RegexOptions.Compiled);

            Program.ExtractOctoParams(search, "#{Octopus.Action[ARM Output].Output.AzureRMOutputs[Foo]}")
                .ShouldOnlyContain(new[] { "Octopus.Action[ARM Output].Output.AzureRMOutputs[Foo]" });
        }
    }
}
