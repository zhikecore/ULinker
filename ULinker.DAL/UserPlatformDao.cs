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
    public class UserPlatformDao
    {
        private static UserPlatformDao _Instance = null;

        public static UserPlatformDao Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new UserPlatformDao();
                return _Instance;
            }
        }

        private UserPlatformDao()
        {

        }

        public bool IsExist(int platformId, int appId)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                int total = 0;
                try
                {
                    string strCmd = @"SELECT COUNT(1) FROM `userplatform`   WHERE `PlatformId`=@PlatformId AND `AppId`=@AppId";

                    MySqlParameter[] paramters = new MySqlParameter[]
            {
                       new MySqlParameter("@PlatformId",MySqlDbType.Int32),
                       new MySqlParameter("@AppId",MySqlDbType.Int32)
            };

                    paramters[0].Value = platformId;
                    paramters[1].Value = appId;

                    DataRow row = db.GetDataRow(strCmd, paramters);
                    if (row != null)
                    {
                        total = int.Parse(row["COUNT(1)"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("PlatformDao.IsExist.{0}", ex.Message), ex);
                }
                return total > 0 ? true : false;
            }
        }

        public bool IsExistAppKey(string appKey)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                int total = 0;
                try
                {
                    string strCmd = @"SELECT COUNT(1) FROM `userplatform`   WHERE `AppKey`=@AppKey";

                    MySqlParameter[] paramters = new MySqlParameter[]
            {
                       new MySqlParameter("@AppKey",MySqlDbType.String)
            };

                    paramters[0].Value = appKey;

                    DataRow row = db.GetDataRow(strCmd, paramters);
                    if (row != null)
                    {
                        total = int.Parse(row["COUNT(1)"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("PlatformDao.IsExistAppKey.{0}", ex.Message), ex);
                }
                return total > 0 ? true : false;
            }

        }

        public UserPlatform GetById(int id)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                UserPlatform item = null;
                try
                {
                    string strCmd = @"SELECT * FROM `userplatform` WHERE `Id`=@Id";
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
                    Log4Helper.Error(this.GetType(), String.Format("UserPlatformDao.GetById.{0}", ex.Message), new Exception("error"));
                }

                return item;
            }
        }

        public UserPlatform GetByAppKey(string appkey,string appsecrect)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                UserPlatform item = null;
                try
                {
                    string strCmd = @"SELECT * FROM `userplatform` WHERE `AppKey`=@AppKey AND `AppSecrect`=@AppSecrect";
                    MySqlParameter[] paramters = new MySqlParameter[]
                {
                       new MySqlParameter("@AppKey",MySqlDbType.String),
                       new MySqlParameter("@AppSecrect",MySqlDbType.String)
                };

                    paramters[0].Value = appkey;
                    paramters[1].Value = appsecrect;

                    DataRow dr = db.GetDataRow(strCmd, paramters);
                    if (dr != null)
                    {
                        item = RowToObject(dr);
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("UserPlatformDao.GetByAppKey.{0}", ex.Message), new Exception("error"));
                }

                return item;
            }
        }

        public List<UserPlatform> GetBySomeWhere(int appId, int platformId, int limit, int pageSize, out int total)
        {
            List<UserPlatform> filtered = new List<UserPlatform>();
            try
            {
                total = 0;
                filtered = _Filter(appId, platformId, limit, pageSize);
                total = Count(appId, platformId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return filtered;
        }

        public bool Create(UserPlatform model)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                try
                {
                    string strCmd = @"INSERT INTO `userplatform`(`CreatorId`,`PlatformId`,`AppId`,`AppKey`,`AppSecrect`,`CreateTime`,`ModifyTime`)
                                      VALUES(@CreatorId,@PlatformId,@AppId,@AppKey,@AppSecrect,NOW(),NOW())";

                    MySqlParameter[] paramters = new MySqlParameter[]
             {
                       new MySqlParameter("@CreatorId",MySqlDbType.Int32),
                       new MySqlParameter("@PlatformId",MySqlDbType.Int32),
                       new MySqlParameter("@AppId",MySqlDbType.Int32),
                       new MySqlParameter("@AppKey",MySqlDbType.String),
                       new MySqlParameter("@AppSecrect",MySqlDbType.String)
             };

                    paramters[0].Value = model.CreatorId;
                    paramters[1].Value = model.PlatformId;
                    paramters[2].Value = model.AppId;
                    paramters[3].Value = model.AppKey;
                    paramters[4].Value = model.AppSecrect;

                    int count = db.ExecuteNonQuery(strCmd, paramters);
                    return count > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("UserPlatformDao.Create.{0}", ex.Message), new Exception("error"));
                    return false;
                }
            }
        }

        public bool Update(UserPlatform model)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                try
                {
                    string strCmd = @"UPDATE `userplatform` 
                                      SET `PlatformId`=@PlatformId,`AppId`=@AppId,`AppKey`=@AppKey,`AppSecrect`=@AppSecrect,`ModifyTime`=NOW()
                                      WHERE `Id`=@Id";

                    MySqlParameter[] paramters = new MySqlParameter[]
             {
                       new MySqlParameter("@PlatformId",MySqlDbType.Int32),
                       new MySqlParameter("@AppId",MySqlDbType.Int32),
                       new MySqlParameter("@AppKey",MySqlDbType.String),
                       new MySqlParameter("@AppSecrect",MySqlDbType.String),
                       new MySqlParameter("@Id",MySqlDbType.Int32),
             };

                    paramters[0].Value = model.PlatformId;
                    paramters[1].Value = model.AppId;
                    paramters[2].Value = model.AppKey;
                    paramters[3].Value = model.AppSecrect;
                    paramters[4].Value = model.Id;

                    int count = db.ExecuteNonQuery(strCmd, paramters);
                    return count > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("UserPlatformDao.Update.{0}", ex.Message), new Exception("error"));
                    return false;
                }
            }
        }

        public bool UpdateToken(string appkey, string token, DateTime tokenExpireTime)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                try
                {
                    string strCmd = @"UPDATE `userplatform` SET `Token`=@Token,`TokenExpireTime`=@TokenExpireTime WHERE `AppKey`=@AppKey";

                    MySqlParameter[] paramters = new MySqlParameter[]
             {
                       new MySqlParameter("@Token",MySqlDbType.String),
                       new MySqlParameter("@AppKey",MySqlDbType.String),
                       new MySqlParameter("@TokenExpireTime",MySqlDbType.DateTime)
             };

                    paramters[0].Value = token;
                    paramters[1].Value = appkey;
                    paramters[2].Value = tokenExpireTime;

                    int count = db.ExecuteNonQuery(strCmd, paramters);
                    return count > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("UserPlatformDao.UpdateToken.{0}", ex.Message), new Exception("error"));
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

        private List<UserPlatform> _Filter(int appId, int platformId, int limit, int pageSize)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                List<UserPlatform> _Platforms = new List<UserPlatform>();
                try
                {
                    string strCmd = @"SELECT * FROM `userplatform` WHERE 1=1";

                    if (platformId != 0)
                    {
                        strCmd += @"  AND `PlatformId`=@PlatformId";
                    }

                    if (appId != 0)
                    {
                        strCmd += @"  AND `AppId`=@AppId";
                    }

                    strCmd += " ORDER BY CreateTime DESC";
                    strCmd += " LIMIT " + limit + "," + pageSize;

                    MySqlParameter[] paramters = new MySqlParameter[]
            {
                       new MySqlParameter("@PlatformId",MySqlDbType.Int32),
                       new MySqlParameter("@AppId",MySqlDbType.Int32)
            };

                    paramters[0].Value = platformId;
                    paramters[1].Value = appId;

                    DataTable dt = db.GetDataSet(strCmd, paramters);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            UserPlatform item = RowToObject(row);
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

        private int Count(int appId, int platformId)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                int total = 0;
                try
                {
                    string strCmd = @" SELECT COUNT(1) FROM `platform` WHERE 1=1 ";

                    if (platformId != 0)
                    {
                        strCmd += @"  AND `PlatformId`=@PlatformId";
                    }

                    if (appId != 0)
                    {
                        strCmd += @"  AND `AppId`=@AppId";
                    }

                    MySqlParameter[] paramters = new MySqlParameter[]
            {
                       new MySqlParameter("@PlatformId",MySqlDbType.Int32),
                       new MySqlParameter("@AppId",MySqlDbType.Int32)
            };

                    paramters[0].Value = platformId;
                    paramters[1].Value = appId;

                    DataRow row = db.GetDataRow(strCmd, paramters);
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
        private UserPlatform RowToObject(DataRow row)
        {
            return new UserPlatform
            {
                Id = int.Parse(row["Id"].ToString()),
                CreatorId = int.Parse(row["CreatorId"].ToString()),
                PlatformId = int.Parse(row["PlatformId"].ToString()),
                AppId = int.Parse(row["AppId"].ToString()),
                AppKey = row["AppKey"].ToString(),
                AppSecrect = row["AppSecrect"].ToString(),
                Token = row["Token"].ToString(),
                IsUse = bool.Parse(row["IsUse"].ToString()),
                TokenExpireTime = row.IsNull("TokenExpireTime") ? new DateTime() : DateTime.Parse(row["TokenExpireTime"].ToString()),
                CreateTime = row.IsNull("CreateTime") ? new DateTime() : DateTime.Parse(row["CreateTime"].ToString()),
                ModifyTime = row.IsNull("ModifyTime") ? new DateTime() : DateTime.Parse(row["ModifyTime"].ToString()),
                Description = row["Description"].ToString()
            };
        }

        #endregion
    }
}
