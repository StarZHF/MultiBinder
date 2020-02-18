using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace MultiBinder
{
    class CodeDOM
    {
        public static string Compile(string sourceCode, string outputPath, string iconPath = null, bool isHidden = true)
        {
            ICodeCompiler compiler = new CSharpCodeProvider().CreateCompiler();
            CompilerParameters parameters = new CompilerParameters();
            CompilerResults results = new CompilerResults(new TempFileCollection());
            Dictionary<string, string> version = new Dictionary<string, string>();
            version.Add("CompilerVersion", "v2.0");
            string errors = "";
            parameters.GenerateExecutable = true;
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.Data.dll");
            parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            parameters.OutputAssembly = outputPath;
            if (isHidden) parameters.CompilerOptions += " /target:winexe";
            if (iconPath != null) parameters.CompilerOptions += " /win32icon:" + iconPath;

            results = compiler.CompileAssemblyFromSource(parameters, sourceCode);

            if(results.Errors.Count != 0)
            {
                foreach(CompilerError cErr in results.Errors)
                {
                    errors += String.Format("[{0}] {1} - Line {2}\r\n", cErr.ErrorNumber, cErr.ErrorText, cErr.Line);
                }
            }
            return errors;
        }
    }
}
