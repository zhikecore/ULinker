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
    public class AppApiDao
    {
        private static AppApiDao _Instance = null;

        public static AppApiDao Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new AppApiDao();
                return _Instance;
            }
        }

        private AppApiDao()
        {

        }
        
        public List<AppApi> GetByAppId(int appId)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                List<AppApi> list = new List<AppApi>();
                try
                {
                    string strCmd = @" SELECT * FROM  `appapi` WHERE `AppId`=@AppId";

                    DataTable dt = db.GetDataSet(strCmd);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            AppApi item = RowToObject(row);
                            list.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("AppApiDao.GetAll.{0}", ex.Message), new Exception("error"));
                }

                return list;
            }
        }

        public bool Create(AppApi model)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                try
                {
                    string strCmd = @"INSERT INTO `appapi` (`AppId`,`ApiId`,`IsUse`,`CreateTime`,`ModifyTime`)
                                      VALUES(@AppId,@ApiId,@IsUse,NOW(),NOW())";

                    MySqlParameter[] paramters = new MySqlParameter[]
             {
                       new MySqlParameter("@AppId",MySqlDbType.Int32),
                       new MySqlParameter("@ApiId",MySqlDbType.Int32),
                       new MySqlParameter("@IsUse",MySqlDbType.Binary)
             };

                    paramters[0].Value = model.Id;
                    paramters[1].Value = model.Description;
                    paramters[2].Value = model.IsUse;

                    int count = db.ExecuteNonQuery(strCmd, paramters);
                    return count > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("AppApiDao.Create.{0}", ex.Message), new Exception("error"));
                    return false;
                }
            }
        }

        /// <summary>
        /// 物理删除App的API
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginAccountId"></param>
        /// <returns></returns>
        public bool PhysicDelete(int appId, string loginAccount)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                try
                {
                    string strCmd = @"DELETE FROM `appapi` WHERE `AppId`=@AppId";

                    MySqlParameter[] paramters = new MySqlParameter[]
             {
                       new MySqlParameter("@AppId",MySqlDbType.Int32)
             };

                    paramters[0].Value = appId;

                    int count = db.ExecuteNonQuery(strCmd, paramters);
                    return count > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("AppApiDao.Create.{0}", ex.Message), new Exception("error"));
                    return false;
                }
            }
        }

        #region Private
        
        private AppApi RowToObject(DataRow row)
        {
            return new AppApi
            {
                Id = int.Parse(row["Id"].ToString()),
                AppId= int.Parse(row["AppId"].ToString()),
                ApiId= int.Parse(row["ApiId"].ToString()),
                IsUse=bool.Parse(row["IsUse"].ToString()),
                CreateTime = row.IsNull("CreateTime") ? new DateTime() : DateTime.Parse(row["CreateTime"].ToString()),
                ModifyTime = row.IsNull("ModifyTime") ? new DateTime() : DateTime.Parse(row["ModifyTime"].ToString())
            };
        }

        #endregion
    }
}
