using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestApp
{
    [DataContract]
    public class Schedule
    {
        [DataMember]
        public Stack<Flight> Flights { get; set; }

        public Schedule()
        {
            Flights = new Stack<Flight>();
        }

        public void Load(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    var jsonString = File.ReadAllText(filePath);
                    JsonConvert.PopulateObject(jsonString, this);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузке файла с расписанием:" + ex.ToString());
                }
            }
            else
                MessageBox.Show("Не найден файл с расписанием");
        }

        public void Save(string filePath)
        {
            try
            {
                var source = JsonConvert.SerializeObject(this, typeof(Schedule), new JsonSerializerSettings
                {
                    MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead,
                    TypeNameHandling = TypeNameHandling.Auto
                });

                File.WriteAllText(filePath, source);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении расписания в файл '" + filePath  + "' :" + ex.ToString());
            }
        }

    }
}
