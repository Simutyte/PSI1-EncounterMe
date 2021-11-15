// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System.Collections.Generic;
using System.Text;
using System.IO;
using SQLite;
using System;

namespace EncounterMe.Users
{
    public class UserDB
    {
        private SQLiteConnection _mySQLiteConnection;
        public int? CurrentUserId { get; set; }
        public UserDB()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            _mySQLiteConnection = new SQLiteConnection(dbPath);
            _mySQLiteConnection.CreateTable<User>();
            CurrentUserId = -1;
        }

        public User GetUserByID(int id)
        {
            return _mySQLiteConnection.Table<User>().Where(i => i.ID == id).FirstOrDefault();
        }

        public void DeleteUser(int id)
        {
            _mySQLiteConnection.Delete<User>(id);
        }

        public bool AddUser(User user)
        {
            var data = _mySQLiteConnection.Table<User>();
            var d1 = data.Where(x => x.Username == user.Username || x.Email == user.Email).FirstOrDefault();
            if (d1 == null)
            {
                _mySQLiteConnection.Insert(user);
                return true;
            }
            else
                return false;
        }

        public bool LoginValidate(string username, string pass)
        {
            var data = _mySQLiteConnection.Table<User>();
            var d1 = data.Where(x => x.Username == username && x.Password == pass).FirstOrDefault();

            if (d1 != null)
            {
                CurrentUserId = d1.ID;
                return true;
            }
            else
                return false;
        }
    }
}
