using MySql.Data.MySqlClient;
using SysInfoManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ULinker.Common.Utility;
using ULinker.Models.DO;

namespace ULinker.DAL
{
    public class AppDao
    {
        private static AppDao _Instance = null;

        public static AppDao Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new AppDao();
                return _Instance;
            }
        }

        private AppDao()
        {

        }

        public App GetById(int id)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                App item = null;
                try
                {
                    string strCmd = @"SELECT * FROM `app` WHERE `Id`=@Id";
                    MySqlParameter[] paramters = new MySqlParameter[]
                {
                       new MySqlParameter("@Id",MySqlDbType.Int32)
                };

                    paramters[0].Value = id;

                    DataRow dr = db.GetDataRow(strCmd, paramters);
                    if (dr != null)
                    {
                        item = RowToObject(dr);
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("AppDao.GetById.{0}", ex.Message), new Exception("error"));
                }

                return item;
            }
        }

        public List<App> GetAll()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                List<App> _Apps = new List<App>();
                try
                {
                    string strCmd = @" SELECT * FROM `app`";

                    DataTable dt = db.GetDataSet(strCmd);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            App item = RowToObject(row);
                            _Apps.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("PlatformDao.GetAll.{0}", ex.Message), new Exception("error"));
                }

                return _Apps;
            }
        }

        public List<App> GetBySomeWhere(string keyword, int limit, int pageSize, out int total)
        {
            List<App> filtered = new List<App>();
            try
            {
                total = 0;
                filtered = _Filter(keyword, limit, pageSize);
                total = Count(keyword);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return filtered;
        }

        public bool Create(App model)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                try
                {
                    string strCmd = @"INSERT INTO `app` (`AppTypeId`,`Name`,`CreatorId`,`ManagerId`,`Description`,`CreateTime`,`ModifyTime`)
                                      VALUES(@AppTypeId,@Name,@CreatorId,@ManagerId,@Description,NOW(),NOW())";

                    MySqlParameter[] paramters = new MySqlParameter[]
             {
                       new MySqlParameter("@AppTypeId",MySqlDbType.Int32),
                       new MySqlParameter("@Name",MySqlDbType.String),
                       new MySqlParameter("@CreatorId",MySqlDbType.Int32),
                       new MySqlParameter("@ManagerId",MySqlDbType.Int32),
                       new MySqlParameter("@Description",MySqlDbType.String)
             };

                    paramters[0].Value = model.AppTypeId;
                    paramters[1].Value = model.Name;
                    paramters[2].Value = model.CreatorId;
                    paramters[3].Value = model.ManagerId;
                    paramters[4].Value = model.Description;

                    int count = db.ExecuteNonQuery(strCmd, paramters);
                    return count > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("AppDao.Create.{0}", ex.Message), new Exception("error"));
                    return false;
                }
            }
        }

        public bool Update(App model)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                try
                {
                    string strCmd = @"UPDATE `app` SET `AppTypeId`=@AppTypeId,`ManagerId`=@ManagerId,`Name`=@Name,`Description`=@Description,`ModifyTime`=NOW() WHERE `Id`=@Id";

                    MySqlParameter[] paramters = new MySqlParameter[]
             {
                       new MySqlParameter("@AppTypeId",MySqlDbType.Int32),
                       new MySqlParameter("@ManagerId",MySqlDbType.Int32),
                       new MySqlParameter("@Name",MySqlDbType.String),
                       new MySqlParameter("@Description",MySqlDbType.String),
                       new MySqlParameter("@Id",MySqlDbType.Int32),
             };

                    paramters[0].Value = model.AppTypeId;
                    paramters[1].Value = model.ManagerId;
                    paramters[2].Value = model.Name;
                    paramters[3].Value = model.Description;
                    paramters[4].Value = model.Id;

                    int count = db.ExecuteNonQuery(strCmd, paramters);
                    return count > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("AppDao.Update.{0}", ex.Message), new Exception("error"));
                    return false;
                }
            }
        }


        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginAccountId"></param>
        /// <returns></returns>
        public bool PhysicDelete(int id, string loginAccount)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                try
                {
                    string strCmd = @"DELETE FROM `app` WHERE `Id`=@Id";

                    MySqlParameter[] paramters = new MySqlParameter[]
             {
                       new MySqlParameter("@Id",MySqlDbType.Int32)
             };

                    paramters[0].Value = id;

                    int count = db.ExecuteNonQuery(strCmd, paramters);
                    return count > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("AppDao.Create.{0}", ex.Message), new Exception("error"));
                    return false;
                }
            }
        }

        #region Private

        private List<App> _Filter(string keyword, int limit, int pageSize)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                List<App> list = new List<App>();
                try
                {
                    string strCmd = @"SELECT * FROM `app` WHERE 1=1";

                    if (!string.IsNullOrEmpty(keyword))
                    {
                        strCmd += @" AND `Name` like " + string.Format("%{0}%", keyword);
                    }


                    strCmd += " ORDER BY CreateTime DESC";
                    strCmd += " LIMIT " + limit + "," + pageSize;

                    DataTable dt = db.GetDataSet(strCmd);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            App item = RowToObject(row);
                            list.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("AppDao._Filter.{0}", ex.Message), new Exception("error"));
                }
                return list;
            }
        }

        private int Count(string name)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                int total = 0;
                try
                {
                    string strCmd = @" SELECT COUNT(1) FROM `app` WHERE 1=1 ";

                    if (!String.IsNullOrEmpty(name))
                    {
                        strCmd += @" AND `Name` LIKE '%" + name + "%'";
                    }

                    DataRow row = db.GetDataRow(strCmd);
                    if (row != null)
                    {
                        total = int.Parse(row["COUNT(1)"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("AppDao.Count.{0}", ex.Message), ex);
                }
                return total;
            }
        }

        private App RowToObject(DataRow row)
        {
            return new App
            {
                Id = int.Parse(row["Id"].ToString()),
                AppTypeId= int.Parse(row["AppTypeId"].ToString()),
                Name = row["Name"].ToString(),
                CreatorId= int.Parse(row["CreatorId"].ToString()),
                ManagerId= int.Parse(row["ManagerId"].ToString()),
                CreateTime = row.IsNull("CreateTime") ? new DateTime() : DateTime.Parse(row["CreateTime"].ToString()),
                ModifyTime = row.IsNull("ModifyTime") ? new DateTime() : DateTime.Parse(row["ModifyTime"].ToString()),
                Description = row["Description"].ToString()
            };
        }

        #endregion
    }
}
