using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Seq2SeqLearn.Tools
{
    class FileBinary
    {
        private BinaryFormatter bf = new BinaryFormatter();
        private string filepath;

        public FileBinary(string filepath)
        {
            this.filepath = filepath;
        }

        public object Read()
        {
            try
            {
                using (var fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                    if (fs.Length > 0)
                        return bf.Deserialize(fs);
            }
            catch (Exception) { }
            return null;
        }

        public bool Write(object obj)
        {
            try
            {
                using (var fs = new FileStream(filepath, FileMode.Create, FileAccess.Write))
                    bf.Serialize(fs, obj);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsExist()
        {
            return File.Exists(filepath);
        }

        public bool Delete()
        {
            try
            {
                File.Delete(filepath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
