using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WeekSevenAssignmentMoreDataBases.Engines;
using WeekSevenAssignmentMoreDataBases.StuffForFiles;
using System.Xml.Serialization;

namespace WeekSevenAssignmentMoreDataBases.StuffForFiles
{
    internal class FileParse
    {
        //Most of the code is remebered from Assignment 4/5

        //List for the files to process
        List<IFile> ProcessingFiles { get; set; }
       
        //List to help check for errors, learned in week 5 in class demo
        List<ErrorCheck> ShowError = new List<ErrorCheck>();
        RegularEngine engineParse { get; set; }    

        
        public FileParse()
        {
            ProcessingFiles = new List<IFile>();
            ShowError = new List<ErrorCheck>();
        }
        private void CreateFiles(List<string> files)
        {
            foreach (var file in files)
            {
                //It adds the files from the folder to the list of ProcessingFiles to get Processed
                if (file.EndsWith(FileExtendAndDeLimit.FileExtensions.CSV))
                {
                    TheFiles resultFile = new TheFiles(file, FileExtendAndDeLimit.FileExtensions.CSV, FileExtendAndDeLimit.FileDelimiter.CSV);
                    ProcessingFiles.Add(resultFile);
                }
                else
                {
                    //Checks if the files is supported for the parsing 
                    ShowError.Add(new ErrorCheck($"Invalid File Extension, {file.Substring(file.LastIndexOf("."))} is not supported", $"{file}"));
                    break;
                }
            }
        }

        public void ParsingFiles(string directPath)
        {
            //Learned from: https://learn.microsoft.com/en-us/dotnet/api/system.environment.newline?view=net-7.0
            Console.WriteLine($"The files are getting processed! {Environment.NewLine}");

            List<string> files = GetFilesToProcess(directPath);

            if (ShowError.Count == 0)
            {
                CreateFiles(files);
            }

            if (ShowError.Count == 0)
            {
                //Goes thorough the files in the Files folder and based on the file extension
                foreach (var file in ProcessingFiles)
                {
                    //Learned from week 4 In Class Demo for processing the different file type with different engine
                    switch (file.Extension)
                    {
                        case ".csv":
                            engineParse = new SQLEngine();
                            ShowError.AddRange(engineParse.ProcessFile(file));
                            break;
                        default:
                            break;
                    }
                }
            }
            //if there is a problem with the engine, it will read out the error and the source before going to the engine
            if (ShowError.Count > 0)
            {
                Console.WriteLine("The process didn't work!");
                foreach (var error in ShowError)
                {
                    Console.WriteLine($"Error Message: {error.ErrorMessage}. Error Source: {error.ErrorSource}");
                }
            }
            else
            {
                //This will write out if there is no problem with the process work
                Console.WriteLine("The file process work successfully!");
            }
        }

        private List<string> GetFilesToProcess(string directPath)
        {
            List<string> outputfile = new List<string>();

            try
            {
                //Takes the list of string and puts it into the _out file
                outputfile = Directory.GetFiles(directPath).Where(x => !x.Contains("_out")).ToList();
            }
            //To catch errors that are in the code, learned from in class demo in week 5
            catch (IOException ioe)
            {
                ShowError.Add(new ErrorCheck(ioe.Message, ioe.Source ?? "Unknown"));
            }
            catch (Exception ex)
            {
                ShowError.Add(new ErrorCheck(ex.Message, ex.Source ?? "Unknown"));
            }

            return outputfile;
        }
    }
}
