using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LISTemplate
{
    class Program
    {
        static string homeFilename = "-home";
        static string formFilename = "-form";
        static string e2eFilename = ".component.e2e-spec.ts";
        static string specFilename = ".component.spec.ts";
        static string htmlFilename = ".component.html";
        static string tsFilename = ".component.ts";
        static string routesFilename = ".routes.ts";
        static string indexFilename = "index.ts";
        static string e2eFoldername = "e2e-tests";

        static string templateOriginalPath;
        static FileType actualType;

        static string inputPath;
        static string inputFilename;
        static FileType inputType;

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                ////test parameters
                //inputType = FileType.Form;
                //inputPath = @"C:\temp\dr";
                //inputFilename = "folio-list";

                //get the folder for the templates
                templateOriginalPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"templates");

                //instructions
                Console.WriteLine("This application will override all files with the same name.");
                Console.WriteLine("You can create just form or homepage files.");
                Console.WriteLine("If you select type 'All' this application will create root, form and homepage files.");
                Console.WriteLine("Press Y to procced:");
                if (Console.ReadLine().ToLower().Equals("y"))
                {
                    while (String.IsNullOrEmpty(inputPath))
                    {
                        //open dialog to select folder
                        Console.WriteLine("Select the DR root folder: ");
                        FolderBrowserDialog fbd = new FolderBrowserDialog();
                        if (fbd.ShowDialog() == DialogResult.OK)
                        {
                            inputPath = fbd.SelectedPath.ToLower();
                            Console.WriteLine(inputPath);
                        }
                    }

                    while (String.IsNullOrEmpty(inputFilename))
                    {
                        //wait the user type the name of the component
                        Console.WriteLine("------------------------");
                        Console.WriteLine("Enter new file name: ");
                        inputFilename = Console.ReadLine().ToLower();
                    }

                    //wait the user type the type of the component
                    Console.WriteLine("------------------------");
                    Console.WriteLine("Enter file type number: ");
                    Console.WriteLine("   for Form type: 1");
                    Console.WriteLine("   for HomePage type: 2");
                    Console.WriteLine("   for Component type: 3");
                    Console.WriteLine("   for All type: 0");
                    Console.WriteLine("   default: All");
                    inputType = (FileType)Convert.ToInt32(Console.ReadLine());

                    CreateStructure(inputPath, inputFilename, inputType);

                    Console.WriteLine("------------------------");
                    Console.WriteLine("FILES CREATED WITH SUCCESS!!");
                    Console.WriteLine("------------------------");
                    Console.WriteLine("Press any key to exit");
                    Console.ReadKey();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("------------------------");
                Console.WriteLine("ERROR");
                Console.WriteLine("------------------------");
                Console.WriteLine(e.Message);
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Based on the type create the files
        /// </summary>
        /// <param name="path">where the files are going to be created</param>
        /// <param name="filename">the name of the files</param>
        /// <param name="fileType">can be a form, a homepage or both</param>
        private static void CreateStructure(string path, string filename, FileType fileType)
        {
            //build the root folder name
            string folderName = Path.Combine(path, filename);
            switch (fileType)
            {
                case FileType.Component:
                    actualType = fileType;
                    folderName = Path.Combine(path, "shared");
                    if (!Directory.Exists(folderName))
                        Directory.CreateDirectory(folderName);
                    folderName = Path.Combine(folderName, filename);
                    CreateStructureFiles(folderName, filename, false);
                    break;
                case FileType.Form:
                    actualType = fileType;
                    //build the form folder name inside the root folder
                    string formFolderName = Path.Combine(folderName, filename + formFilename);
                    //create the e2e file for the form file inside the e2e shared folder
                    CreateE2eFile(path, filename + formFilename);
                    //create all files inside the form folder
                    CreateStructureFiles(formFolderName, filename + formFilename, false);
                    break;
                case FileType.HomePage:
                    actualType = fileType;
                    //build the home folder name inside the root folder
                    string homeFolderName = Path.Combine(folderName, "home");
                    //create the e2e file for the home file inside the e2e shared folder
                    CreateE2eFile(path, filename + homeFilename);
                    //create all files inside the home folder
                    CreateStructureFiles(homeFolderName, filename + homeFilename, true);
                    break;
                default:
                case FileType.All:
                    actualType = fileType;
                    //create the root directory
                    Directory.CreateDirectory(folderName);
                    //create the e2e file inside the e2e shared folder
                    CreateE2eFile(path, filename);
                    //create all files insite the root folder
                    CreateStructureFiles(folderName, filename, true);
                    //call this method again to create the form files
                    CreateStructure(path, filename, FileType.Form);
                    //call this method again to create the home files
                    CreateStructure(path, filename, FileType.HomePage);
                    break;
            }
        }

        /// <summary>
        /// this method is responsable for build the structure files
        /// </summary>
        /// <param name="path">the folder where the files should be created</param>
        /// <param name="filename">the name of the file</param>
        /// <param name="createRoutes">true if should create a routes file</param>
        private static void CreateStructureFiles(string path, string filename, bool createRoutes)
        {
            //create folder
            Directory.CreateDirectory(path);

            //create html file
            //read the html template file to get the content
            string[] content = UpdateTemplateFile(filename + htmlFilename);
            //create the new html file with content of the template or an empty file
            CreateFile(path, filename + htmlFilename, content);

            //create spec file
            //read the spec template file to get the content
            content = UpdateTemplateFile(filename + specFilename);
            //create the new spec file with content of the template or an empty file
            CreateFile(path, filename + specFilename, content);

            //create ts file
            //read the ts template file to get the content
            content = UpdateTemplateFile(filename + tsFilename);
            //create the new ts file with content of the template or an empty file
            CreateFile(path, filename + tsFilename, content);

            //create routes file
            if (createRoutes)
            {
                //read the routes template file to get the content
                content = UpdateRoutesFile(filename + routesFilename);
                //create the new routes file with content of the template or an empty file
                CreateFile(path, filename + routesFilename, content);
            }

            //create index file
            //read the index template file to get the content
            content = UpdateIndexFile(indexFilename, path);
            //create the new index file with content of the template or an empty file
            CreateFile(path, indexFilename, content);
        }

        /// <summary>
        /// Create the e2e file in the e2e shared folder
        /// </summary>
        /// <param name="path">root of e2e shared folder</param>
        /// <param name="filename">name of the file</param>
        private static void CreateE2eFile(string path, string filename)
        {
            //create e2e file
            string e2eFolderPath = Path.Combine(path, e2eFoldername);
            
            //check if e2e folder does not exist and create
            if (!Directory.Exists(e2eFolderPath))
                Directory.CreateDirectory(e2eFolderPath);

            //create e2e
            //read the e2e template file to get the content
            string[] content = UpdateE2eFile(filename + e2eFilename);
            //create the new e2e file with content of the template or an empty file
            CreateFile(e2eFolderPath, filename + e2eFilename, content);
        }

        /// <summary>
        /// Create an empty file or a template file in the folder
        /// </summary>
        /// <param name="path">where the file is going to be created</param>
        /// <param name="filename">the name of the file</param>
        /// <param name="fileContent">if the template have some content</param>
        private static void CreateFile(string path, string filename, string[] fileContent)
        {
            //combine the path with filename
            string file = Path.Combine(path,filename);
            
            //check if the content exist to create the new file
            if (fileContent != null && fileContent.Length > 0)
                File.WriteAllLines(file, fileContent);
            else
                //create an empty file
                File.Create(file);
        }

        /// <summary>
        /// Read the respective template file and replace the parameters for the filename
        /// </summary>
        /// <param name="filename">The name of the file</param>
        /// <returns>The content of the file to be created</returns>
        private static string[] UpdateTemplateFile(string filename)
        {
            string[] lines = null;

            string templatePath = GetTemplatePath(filename);
            //replace the name fof the file for x to get the template name
            string templateName = filename.Replace(inputFilename, "x");
            templateName = templateName.Replace(formFilename, "").Replace(homeFilename, "");

            //get the name of the file to replace inside the file and append 'Component' at the end of the name
            string name = SplitUpperCase(inputFilename) + "Component";
                        
            //read all lines of the template file
            lines = System.IO.File.ReadAllLines(Path.Combine(templatePath, templateName));
            //replace all occurrences of #name for the name build above
            lines = lines.Select(x => x.Replace("#name", name)).ToArray();
            //replace all occurrences of #path for the raw name build above
            lines = lines.Select(x => x.Replace("#path", inputFilename)).ToArray();

            return lines;
        }

        /// <summary>
        /// Read the respective routes template file and replace the parameters for the filename
        /// </summary>
        /// <param name="filename">The name of the file</param>
        /// <returns>The content of the file to be created</returns>
        private static string[] UpdateRoutesFile(string filename)
        {
            string[] lines = null;

            string templatePath = GetTemplatePath(filename);
            string templateName = filename.Replace(inputFilename, "x");
            templateName = templateName.Replace(formFilename, "").Replace(homeFilename, "");
            lines = System.IO.File.ReadAllLines(Path.Combine(templatePath, templateName));

            string clearCapsName = SplitUpperCase(inputFilename);
            string name = clearCapsName.Replace(formFilename, "Form").Replace(homeFilename, "Home") + "Routes";
            clearCapsName = clearCapsName.Replace(homeFilename, "").Replace(formFilename, "");
            string routeName = clearCapsName + "Home" + "Routes";
            string homeName = clearCapsName + "Home" + "Component";
            string formName = clearCapsName + "Form" + "Component";
            string rootCompName = clearCapsName + "Component";
            string clearName = inputFilename.Replace(homeFilename, "").Replace(formFilename, "");

            lines = lines.Select(x => x.Replace("#name", name)).ToArray();
            lines = lines.Select(x => x.Replace("#formName", formName)).ToArray();
            lines = lines.Select(x => x.Replace("#homeName", homeName)).ToArray();
            lines = lines.Select(x => x.Replace("#routeName", routeName)).ToArray();
            lines = lines.Select(x => x.Replace("#clearName", clearName)).ToArray();
            lines = lines.Select(x => x.Replace("#rootCompName", rootCompName)).ToArray();

            return lines;
        }

        /// <summary>
        /// Read the respective index template file and replace the parameters for the filename
        /// </summary>
        /// <param name="filename">The name of the file</param>
        /// <returns>The content of the file to be created</returns>
        private static string[] UpdateIndexFile(string filename, string indexName)
        {
            string[] lines = null;

            string templatePath = GetTemplatePath(indexName);
            lines = System.IO.File.ReadAllLines(Path.Combine(templatePath, filename));

            string name = SplitUpperCase(inputFilename);
            name = name.Replace(formFilename, "Form").Replace(homeFilename, "Home");
            string[] tempIndexName = indexName.Replace(@"\", @"/").Split('/');
            indexName = tempIndexName.Last();
            if (indexName.Equals("home"))
                indexName = tempIndexName[tempIndexName.Length - 2] + homeFilename;
            
            lines = lines.Select(x => x.Replace("#name", name)).ToArray();
            lines = lines.Select(x => x.Replace("#path", inputFilename)).ToArray();
            lines = lines.Select(x => x.Replace("#indexName", indexName)).ToArray();

            return lines;
        }

        /// <summary>
        /// Read the respective e2e template file and replace the parameters for the filename
        /// </summary>
        /// <param name="filename">The name of the file</param>
        /// <returns>The content of the file to be created</returns>
        private static string[] UpdateE2eFile(string filename)
        {
            string[] lines = null;

            string templateName = filename.Replace(inputFilename, "x");
            lines = System.IO.File.ReadAllLines(Path.Combine(templateOriginalPath, "e2e", templateName));

            string name = "";
            lines = lines.Select(x => x.Replace("#name", name)).ToArray();

            return lines;
        }

        /// <summary>
        /// Transform the first letter of the string to upper case
        /// </summary>
        /// <param name="input">the string that is going to be modified</param>
        /// <returns>the same input string but with first letter upper case</returns>
        private static string FirstUpperCase(string input)
        {
            char[] a = input.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        /// <summary>
        /// Transfomr all first letters after a dash to upper case and remove all dashes
        /// </summary>
        /// <param name="input">string that is going to be modified</param>
        /// <returns>same input string with upper case letters and without dashes</returns>
        private static string SplitUpperCase(string input)
        {
            string ret = string.Empty;
            string[] aux = input.Split('-');
            for (int i = 0; i < aux.Length; i++)
            {
                string item = aux[i];
                ret += FirstUpperCase(item);
            }

            return ret;

        }

        /// <summary>
        /// Based on the name of the file get the right template path
        /// </summary>
        /// <param name="filename">The name of the file</param>
        /// <returns>the full path for the respective template file</returns>
        private static string GetTemplatePath(string filename)
        {
            string templatePath = string.Empty;

            switch (actualType)
            {
                case FileType.Form:
                    templatePath = Path.Combine(templateOriginalPath, "form");
                    break;
                case FileType.HomePage:
                    templatePath = Path.Combine(templateOriginalPath, "home");
                    break;
                case FileType.Component:
                    templatePath = Path.Combine(templateOriginalPath, "component");
                    break;
                case FileType.All:
                default:
                    //if (filename.Contains("home"))
                    //    templatePath = Path.Combine(templateOriginalPath, "home");
                    //else if (filename.Contains(formFilename))
                    //    templatePath = Path.Combine(templateOriginalPath, "form");
                    //else
                        templatePath = Path.Combine(templateOriginalPath, "root");
                    break;
            }

            return templatePath;
        }

        public enum FileType
        {
            All = 0,
            Form = 1,
            HomePage = 2,
            Component = 3
        }
    }
}
