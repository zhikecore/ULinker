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
    public class PlatformDao
    {
        private static PlatformDao _Instance = null;

        public static PlatformDao Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new PlatformDao();
                return _Instance;
            }
        }

        private PlatformDao()
        {

        }

        public Platform GetById(int id)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                Platform item = null;
                try
                {
                    string strCmd = @"SELECT * FROM `platform` WHERE `Id`=@Id";
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
                    Log4Helper.Error(this.GetType(), String.Format("PlatformDao.GetById.{0}", ex.Message), new Exception("error"));
                }

                return item;
            }
        }

        public List<Platform> GetAll()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db =new Common.DB.DBHelper(conStr))
            {
                List<Platform> _Plaforms = new List<Platform>();
                try
                {
                    string strCmd = @" SELECT * FROM `platform`";

                    DataTable dt = db.GetDataSet(strCmd);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            Platform item = RowToObject(row);
                            _Plaforms.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("PlatformDao.GetAll.{0}", ex.Message), new Exception("error"));
                }

                return _Plaforms;
            }
        }

        public List<Platform> GetBySomeWhere(string keyword, int limit, int pageSize, out int total)
        {
            List<Platform> filtered = new List<Platform>();
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

        public bool Create(Platform model)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                try
                {
                    string strCmd = @"INSERT INTO `platform` (`Name`,`Description`,`CreateTime`,`ModifyTime`)
                                      VALUES(@Name,@Description,NOW(),NOW())";

                    MySqlParameter[] paramters = new MySqlParameter[]
             {
                       new MySqlParameter("@Name",MySqlDbType.String),
                       new MySqlParameter("@Description",MySqlDbType.String)
             };

                    paramters[0].Value = model.Name;
                    paramters[1].Value = model.Description;

                    int count = db.ExecuteNonQuery(strCmd, paramters);
                    return count > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("PlatformDao.Create.{0}", ex.Message), new Exception("error"));
                    return false;
                }
            }
        }


        public bool Update(Platform model)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                try
                {
                    string strCmd = @"UPDATE  `platform` SET `Name`=@Name,`Description`=@Description,`ModifyTime`=NOW() WHERE `Id`=@Id";

                    MySqlParameter[] paramters = new MySqlParameter[]
             {
                       new MySqlParameter("@Name",MySqlDbType.String),
                       new MySqlParameter("@Description",MySqlDbType.String),
                       new MySqlParameter("@Id",MySqlDbType.Int32),
             };
                    
                    paramters[0].Value = model.Name;
                    paramters[1].Value = model.Description;
                    paramters[2].Value = model.Id;

                    int count = db.ExecuteNonQuery(strCmd, paramters);
                    return count > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("PlatformDao.Update.{0}", ex.Message), new Exception("error"));
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
        public bool PhysicDelete(int id, string loginAccountId)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                try
                {
                    string strCmd = @"DELETE FROM `platform` WHERE `Id`=@Id";

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
                    Log4Helper.Error(this.GetType(), String.Format("PlatformDao.Create.{0}", ex.Message), new Exception("error"));
                    return false;
                }
            }
        }

        #region Private

        private List<Platform> _Filter(string keyword, int limit, int pageSize)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                List<Platform> _Platforms = new List<Platform>();
                try
                {
                    string strCmd = @"SELECT * FROM `platform` WHERE 1=1";

                    if (!string.IsNullOrEmpty(keyword))
                    {
                        strCmd += @" AND `Name` like " + string.Format("'%{0}%'", keyword);
                    }


                    strCmd += " ORDER BY CreateTime DESC";
                    strCmd += " LIMIT " + limit + "," + pageSize;

                    DataTable dt = db.GetDataSet(strCmd);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            Platform item = RowToObject(row);
                            _Platforms.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("PlatformDao._Filter.{0}", ex.Message), new Exception("error"));
                }
                return _Platforms;
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
                    string strCmd = @" SELECT COUNT(1) FROM `platform` WHERE 1=1 ";

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
                    Log4Helper.Error(this.GetType(), String.Format("PlatformDao.Count.{0}", ex.Message), ex);
                }
                return total;
            }
        }
        private Platform RowToObject(DataRow row)
        {
            return new Platform
            {
                Id = int.Parse(row["Id"].ToString()),
                Name = row["Name"].ToString(),
                CreateTime = row.IsNull("CreateTime") ? new DateTime() : DateTime.Parse(row["CreateTime"].ToString()),
                ModifyTime = row.IsNull("ModifyTime") ? new DateTime() : DateTime.Parse(row["ModifyTime"].ToString()),
                Description = row["Description"].ToString()
            };
        }

        #endregion
    }
}
