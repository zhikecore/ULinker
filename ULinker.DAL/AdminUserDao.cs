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
    public class AdminUserDao
    {
        private static AdminUserDao _Instance = null;

        public static AdminUserDao Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new AdminUserDao();
                return _Instance;
            }
        }

        private AdminUserDao()
        {

        }
        
        public AdminUser GetById(int id)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                AdminUser item = null;
                try
                {
                    string strCmd = @"SELECT * FROM `user` WHERE `Id`=@Id";
                    MySqlParameter[] paramters = new MySqlParameter[]
                {
                       new MySqlParameter("@Id",MySqlDbType.String)
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
                    Log4Helper.Error(this.GetType(), String.Format("AdminUserDao.GetById.{0}", ex.Message), new Exception("error"));
                }

                return item;
            }
        }
        public AdminUser GetByAccount(string account)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                AdminUser item = null;
                try
                {
                    string strCmd = @"SELECT * FROM `user` WHERE `Account`=@Account";
                    MySqlParameter[] paramters = new MySqlParameter[]
                {
                       new MySqlParameter("@Account",MySqlDbType.String)
                };

                    paramters[0].Value = account;

                    DataRow dr = db.GetDataRow(strCmd, paramters);
                    if (dr != null)
                    {
                        item = RowToObject(dr);
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("AdminUserDao.GetByAccount.{0}", ex.Message), new Exception("error"));
                }

                return item;
            }
        }

        public List<AdminUser> GetAll()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                List<AdminUser> list = new List<AdminUser>();
                try
                {
                    string strCmd = @" SELECT * FROM `user`";

                    DataTable dt = db.GetDataSet(strCmd);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            AdminUser item = RowToObject(row);
                            list.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("AdminUserDao.GetAll.{0}", ex.Message), new Exception("error"));
                }

                return list;
            }
        }

        public List<AdminUser> GetBySomeWhere(string keyword, int limit, int pageSize, out int total)
        {
            List<AdminUser> filtered = new List<AdminUser>();
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

        public bool Create(AdminUser model)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                try
                {
                    string strCmd = @"INSERT INTO `user`(`Id`,`RealName`,`Account`,`Email`,`Phone`,`IsUse`,`Avatar`,`Description`,`CreateTime`,`ModifyTime`)
                                      VALUES(@Id,@RealName,@Account,@Email,@Phone,@IsUse,@Avatar,@Description,NOW(),NOW())";

                    MySqlParameter[] paramters = new MySqlParameter[]
             {
                       new MySqlParameter("@Id",MySqlDbType.Int32),
                       new MySqlParameter("@RealName",MySqlDbType.String),
                       new MySqlParameter("@Account",MySqlDbType.String),
                       new MySqlParameter("@Email",MySqlDbType.String),
                       new MySqlParameter("@Phone",MySqlDbType.String),
                       new MySqlParameter("@IsUse",MySqlDbType.Bit),
                       new MySqlParameter("@Avatar",MySqlDbType.String),
                       new MySqlParameter("@Description",MySqlDbType.String)
             };

                    paramters[0].Value = model.Id;
                    paramters[1].Value = model.RealName;
                    paramters[2].Value = model.Account;
                    paramters[3].Value = model.Email;
                    paramters[4].Value = model.Phone;
                    paramters[5].Value = model.IsUse;
                    paramters[6].Value = model.Avatar;
                    paramters[7].Value = model.Description;

                    int count = db.ExecuteNonQuery(strCmd, paramters);
                    return count > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("AdminUserDao.Create.{0}", ex.Message), new Exception("error"));
                    return false;
                }
            }
        }

        /// <summary>
        /// 设置是否可用
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginAccountId"></param>
        /// <returns></returns>
        public bool CanUse(int id, string loginAccountId)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr_pangu"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                try
                {
                    string strCmd = @"UPDATE `user` SET `IsUse`=IF( `IsUse`=FALSE,TRUE,FALSE) WHERE `Id`=@Id";

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
                    Log4Helper.Error(this.GetType(), String.Format("AdminUserDao.CanUse.{0}", ex.Message), new Exception("error"));
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
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr_pangu"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                try
                {
                    string strCmd = @"DELETE FROM `user` WHERE `Id`=@Id";

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
                    Log4Helper.Error(this.GetType(), String.Format("AdminUserDao.Create.{0}", ex.Message), new Exception("error"));
                    return false;
                }
            }
        }

        #region Private

        private List<AdminUser> _Filter(string keyword, int limit, int pageSize)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                List<AdminUser> list = new List<AdminUser>();
                try
                {
                    string strCmd = @"SELECT * FROM `user` WHERE 1=1";

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
                            AdminUser item = RowToObject(row);
                            list.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("AdminUserDao.GetAll.{0}", ex.Message), new Exception("error"));
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
                    string strCmd = @" SELECT COUNT(1) FROM `user` WHERE 1=1 ";

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
                    Log4Helper.Error(this.GetType(), String.Format("AdminUserDao.Count.{0}", ex.Message), ex);
                }
                return total;
            }
        }

        private AdminUser RowToObject(DataRow row)
        {
            return new AdminUser
            {
                Id = int.Parse(row["Id"].ToString()),
                RealName = row["RealName"].ToString(),
                Account = row["Account"].ToString(),
                Email = row["Email"].ToString(),
                Phone = row["Phone"].ToString(),
                IsUse = bool.Parse(row["IsUse"].ToString()),
                Avatar = row["Avatar"].ToString(),
                CreateTime = row.IsNull("CreateTime") ? new DateTime() : DateTime.Parse(row["CreateTime"].ToString()),
                ModifyTime = row.IsNull("ModifyTime") ? new DateTime() : DateTime.Parse(row["ModifyTime"].ToString()),
                Description = row["Description"].ToString()
            };
        }

        #endregion
    }
}
