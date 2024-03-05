using System.Linq.Dynamic.Core;
using Practice.Models.Data;
using Practice.Models.Report;


namespace Practice.Models
{
    public static class DataWorker
    {
        //Получение данных из БД
        #region GET DATA
        public static List<Abonent> GetAllAbonents()
        {
            using(ModelContext db = new ModelContext())
            {
                var result = db.Abonents.OrderBy(x => x.Id).ToList();
                return result;
            }
        }

        public static List<AbonentType> GetAllAbonentTypes()
        {
            using (ModelContext db = new ModelContext())
            {
                var result = db.AbonentTypes.OrderBy(x => x.Id).ToList();
                return result;
            }
        }

        public static List<AbonentDetail> GetAllAbonentDetails()
        {
            using (ModelContext db = new ModelContext())
            {
                var result = db.AbonentDetails.ToList();
                return result;
            }
        }

        public static List<AbonentService> GetAllAbonentServices()
        {
            using (ModelContext db = new ModelContext())
            {
                var result = db.AbonentServices.ToList();
                return result;
            }
        }
        #endregion

        //linq запросы для отчета
        #region REQUESTS FOR REPORT
        public static List<Report1> GetAllReport1()
        {        
            using (ModelContext db = new ModelContext())
            { 
                var result = from services in db.AbonentServices                            
                              join abonents in db.Abonents on services.Abonent equals abonents.Id
                              join types in db.AbonentTypes on abonents.Ptype equals types.Id
                              select new
                              {
                                  services,
                                  types
                              } into t1
                              group t1 by new { t1.services.Timestamp.Month, t1.types.Name } into g
                              select new Report1
                              {
                                  Month = g.Key.Month,
                                  Name = g.Key.Name,
                                  Cost = g.Sum(x => x.services.Cost)
                              };

                  return result.ToList();
            }
        }

        public static List<Report2> GetAllReport2()
        {
            using (ModelContext db = new ModelContext())
            {
                var result = from details in db.AbonentDetails
                             join abonents in db.Abonents on details.Abonent equals abonents.Id
                             join types in db.AbonentTypes on abonents.Ptype equals types.Id
                             select new
                             {
                                 details,
                                 abonents,
                                 types
                             } into t1
                             group t1 by new { t1.abonents.Pnumber, t1.types.Name, t1.details.Timestamp.Month } into g
                             select new Report2
                             {
                                 Pnumber = g.Key.Pnumber,
                                 Name = g.Key.Name,
                                 Service = g.FirstOrDefault().details.Service,
                                 Timestamp = g.Key.Month,
                                 Duration = g.FirstOrDefault().details.Duration,
                                 Cost = g.Sum(x=>x.details.Cost)
                             };

                var result2 = result.OrderBy(o => o.Name);

                return result2.ToList();
            }
        }

        public static List<Report3> GetAllReport3()
        {
            using (ModelContext db = new ModelContext())
            {
                 var result = from services in db.AbonentServices
                             join abonents in db.Abonents on services.Abonent equals abonents.Id
                             join types in db.AbonentTypes on abonents.Ptype equals types.Id
                             select new
                             {
                                 services,
                                 abonents,
                                 types
                             } into t1
                             group t1 by new { t1.abonents.Pnumber, t1.services.Timestamp.Month} into g
                             select new Report3
                             {
                                 Pnumber = g.Key.Pnumber,
                                 Name = g.FirstOrDefault().types.Name,
                                 Service = g.FirstOrDefault().services.Service,
                                 Timestamp = g.Key.Month,
                                 Duration = g.FirstOrDefault().services.Duration,
                                 Cost = g.Sum(x => x.services.Cost),
                                 CostNds = g.Sum(x => x.services.CostNds),
                                 SumCost = g.Sum(x => x.services.Cost + x.services.CostNds)
                             }; 
                             
                var result2 = result.OrderBy(o => o.Name);

                return result2.ToList();
            }
        }
        #endregion

        //Добавления данных в БД
        #region CREATE DATA
        public static string CreateAbonentType(string name, byte mobile)
        {
            string result = "Уже существует";
            using(ModelContext db = new ModelContext())
            {
                bool checkIsExist = db.AbonentTypes.Any(el => el.Name == name);
                if (!checkIsExist)
                {
                    AbonentType newAbonentType = new AbonentType
                    {
                        Name = name,
                        Mobile = mobile
                    };
                    db.AbonentTypes.Add(newAbonentType);
                    db.SaveChanges();
                    result = "Сделано!";
                }
            }
            return result;
        }

        public static string CreateAbonent(byte country, byte city, int pnumber, byte fax, string description,byte ptype, byte secure)
        {
            string result = "Уже существует";
            using (ModelContext db = new ModelContext())
            {
                bool checkIsExist = db.Abonents.Any(el => el.Country == country && el.City == city && el.Pnumber == pnumber );
                if (!checkIsExist)
                {
                    Abonent newAbonent = new Abonent
                    {
                        Country = country,
                        City = city,
                        Pnumber = pnumber,
                        Fax = fax,
                        Description = description,
                        Ptype = ptype,
                        Secure = secure
                    };
                    db.Abonents.Add(newAbonent);
                    db.SaveChanges();
                    result = "Сделано!";
                }
            }
            return result;
        }
        #endregion

        //Удаление данных в БД
        #region DELETE DATA

        //В данном методе я хотел реализовать удаление всех уникальных элементов которые связаны с AbonentType (связаны ptype из Abonent и abonent из (AbonentDetail и AbonentService)) но у меня немного не получилось реализовать идею
        public static string DeleteAbonentType(AbonentType abonentType)
        {
            string result = "Такого типа абонентов не существует";
            using (ModelContext db = new ModelContext())
            {
                
                /* db.AbonentDetails.RemoveRange(db.AbonentDetails.Join(db.Abonents,
                     d => d.Abonent,
                     a => a.Id,
                     (d, a) => new
                     {
                         a
                     }).Where(r => r.a.Ptype == abonentType.Id));*/
                
 
                db.Abonents.RemoveRange(db.Abonents.Where(r => r.Ptype == abonentType.Id));
                db.AbonentTypes.Remove(abonentType);
                db.SaveChanges();

                result = "Сделано! тип абонентов " + abonentType.Name + " удален";
            }
            return result;
        }

        public static string DeleteAbonent(Abonent abonent)
        {
            string result = "Такого абонента не существует";
            using (ModelContext db = new ModelContext())
            {
                db.AbonentDetails.RemoveRange(db.AbonentDetails.Where(r => r.Abonent == abonent.Id));
                db.AbonentServices.RemoveRange(db.AbonentServices.Where(r => r.Abonent == abonent.Id));
                db.Abonents.Remove(abonent);
                db.SaveChanges();
                
                result = "Сделано! Абонент " + abonent.Country + " " +  abonent.City + " " + abonent.Pnumber + " удален";
            }
            return result;
        }
        #endregion

        //Редактирование данных в БД
        #region EDIT DATA
        public static string EditAbonentType(AbonentType oldAbonentType, string newName, byte newMobile)
        {
            string result = "Такого отдела не существует";
            using (ModelContext db = new ModelContext())
            {
                AbonentType abonentType = db.AbonentTypes.FirstOrDefault(p => p.Id == oldAbonentType.Id);
                abonentType.Name = newName;
                abonentType.Mobile = newMobile;
                db.SaveChanges();

                result = "Сделано! тип абонента " + abonentType.Name + " изменен";
            }
            return result;
        }

        public static string EditAbonent(Abonent oldAbonent, byte newCountry, byte newCity, 
            int newPnumber, byte newFax, string newDescription, byte newPtype, byte newSecure)
        {
            string result = "Такого абонента не существует";
            using (ModelContext db = new ModelContext())
            {
                Abonent abonent = db.Abonents.FirstOrDefault(p => p.Id == oldAbonent.Id);
                abonent.Country = newCountry;
                abonent.City = newCity;
                abonent.Pnumber = newPnumber;
                abonent.Fax = newFax;
                abonent.Description = newDescription;
                abonent.Ptype = newPtype;
                abonent.Secure = newSecure;
                db.SaveChanges();

                result = "Сделано! Абонент " + abonent.Country + " " + abonent.City + " " + abonent.Pnumber + " изменен";
            }
            return result;
        }
        #endregion
    }
}
