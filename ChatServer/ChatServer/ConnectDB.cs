using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    class ConnectDB
    {
        //データベースインスタンス
        private SqlTransaction tran = null;

        //排他制御用ロックオブジェクト
        public object locker;

        //コンストラクタ
        public ConnectDB()
        {

        }

        public bool Initialize()
        {
            try
            {


                return true;
            }
            catch (Exception a)
            {
                return false;
            }
        }

        public bool AddDB(string msg,int id,int toid)
        {
            tran = null;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = "Data Source=(local);User ID=takai;Password=takahiko;Initial Catalog=test_db";
            con.Open();

            try
            {
                tran = con.BeginTransaction();

                //SQL文と接続情報を指定し、MySQLコマンドを作成
                SqlCommand cmd = con.CreateCommand();


                //データベース追加処理を記入
                cmd.CommandText = "INSERT INTO tk.msg VALUES( @id, @toid, @msg, @date ,@flag )";

                cmd.Transaction = tran;

                // パラメータ設定
                cmd.Parameters.Add("@id", MySqlDbType.Int16);
                cmd.Parameters["@id"].Value = id;
                cmd.Parameters.Add("@toid", MySqlDbType.Int16);
                cmd.Parameters["@toid"].Value = toid;
                cmd.Parameters.Add("@msg", MySqlDbType.String);
                cmd.Parameters["@msg"].Value = msg;
                cmd.Parameters.Add("@date", MySqlDbType.DateTime);
                cmd.Parameters["@date"].Value = DateTime.Now;
                cmd.Parameters.Add("@flag", MySqlDbType.Int16);
                cmd.Parameters["@flag"].Value = 0;

                //戻り値なしでコマンドを実行
                cmd.ExecuteNonQuery();

                tran.Commit();
            }
            catch (Exception e)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                return false;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

            return true;
        }

        //タイマーイベントによって呼び出されるメソッド
        public List<string> GetMsg(int id)
        {

            tran = null;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = "Data Source=(local);User ID=takai;Password=takahiko;Initial Catalog=test_db";
            con.Open();

            try
            {

                //SQL文と接続情報を指定し、MySQLコマンドを作成
                SqlCommand cmd = con.CreateCommand();

                //データベース追加処理を記入
                cmd.CommandText = "select * from tk.msg where flag=0 AND toid=" +id+ "order by insert_date asc";

                SqlDataReader reader = cmd.ExecuteReader();

                //配列に格納
                List<string> list= new List<string>();

                //レコードを一つだけ進める
                //送信方法を配列（クラス）に変更する。
                while (reader.Read()) 
                {
                    //msgを取り出す
                    list.Add(reader["msg"].ToString());
                }

                con.Close();
                con.Dispose();

                if (list[0] != "")
                {
                    //フラグの更新
                    UpdateFlag(id);
                }

                return list;
            }
            catch (Exception e)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                //中身が空の配列を返す。
                List<string> list = new List<string>();

                return list;
            }
            finally
            {
                if(con!=null)
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }

        public void UpdateFlag(int id)
        {
            lock (locker)
            {
                tran = null;
                SqlConnection smc = new SqlConnection();
                smc.ConnectionString = "Data Source=(local);User ID=takai;Password=takahiko;Initial Catalog=test_db";
                smc.Open();

                try
                {
                    tran = smc.BeginTransaction();

                    SqlCommand sc = smc.CreateCommand();

                    sc.Transaction = tran;

                    sc.CommandText = "update tk.msg set flag = 1 where flag=0 AND toid=" + id;

                    sc.ExecuteNonQuery();

                    tran.Commit();

                }
                catch (Exception e)
                {

                }
                finally
                {
                    smc.Close();
                    smc.Dispose();
                }
            }
        }
    }
}
