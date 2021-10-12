// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System.Collections.Generic;
using System.Text;
using System.IO;
using SQLite;
using System;

namespace EncounterMe.Users
{
   
    class UserDB
    {
        private SQLiteConnection _mySQLiteConnection;

        public UserDB()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            _mySQLiteConnection = new SQLiteConnection(dbPath);
            _mySQLiteConnection.CreateTable<User>();
        }

        public User GetUserByID(int id)
        {
            return _mySQLiteConnection.Table<User>().Where(i => i.ID == id).FirstOrDefault();
        }

        public void DeleteUser(int id)
        {
            _mySQLiteConnection.Delete<User>(id);
        }

        public string AddUser(User user)
        {
            var data = _mySQLiteConnection.Table<User>();
            var d1 = data.Where(x => x.username == user.username || x.email == user.email).FirstOrDefault();
            if (d1 == null)
            {
                _mySQLiteConnection.Insert(user);
                return "Sucessfully Added";
            }
            else
                return "Username or mail already exist";
        }

        public bool LoginValidate(string username, string pass)
        {
            var data = _mySQLiteConnection.Table<User>();
            var d1 = data.Where(x => x.username == username && x.password == pass).FirstOrDefault();

            if (d1 != null)
            {
                return true;
            }
            else
                return false;
        }
    }
}
