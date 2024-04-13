using Python.Runtime;

namespace ProjetoPDS.Classes
{
    public class ReconhecimentoFacial
    {
        static ReconhecimentoFacial()
        {
            Runtime.PythonDLL = "C:\\Users\\marco\\AppData\\Local\\Programs\\Python\\Python311\\python311.dll";
            PythonEngine.PythonPath = PythonEngine.PythonPath;
        }
        public ReconhecimentoFacial()
        {
        }

        public byte[] getFacialEncoding(string pathToFile)
        {
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic sys = Py.Import("sys");

                // Append the directory containing teste.py to the Python path
                sys.path.append(@"D:\Universidade\2ºAno\PDS\ProjetoPDS\ProjetoPDS\FaceRecognition\face\recognition\");

                dynamic facilRecMod = Py.Import("__init__");
                dynamic loadEncFunc= facilRecMod.recognition;

                dynamic auxEncoding = loadEncFunc(pathToFile);

                byte[] encoding = (byte[])auxEncoding;

                return encoding;

            }
        }
        //public void Shutdown()
        //{
        //    PythonEngine.Shutdown();
        //}
    }
}
