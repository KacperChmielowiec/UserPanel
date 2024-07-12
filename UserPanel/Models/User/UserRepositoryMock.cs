﻿using AutoMapper;
using System.Data;
using UserPanel.Services.observable;
using UserPanel.Interfaces.Abstract;
using UserPanel.References;
using UserPanel.Helpers;
namespace UserPanel.Models.User
{
    public class UserRepositoryMock : UserRepository<UserModel>
    {
        ISession _session;
        private string UserPath = SessionKeysReferences.usersKey;
        public UserRepositoryMock(ISession session)
        {
            _session = session;
        }
        public override UserModel? GetModelById(int id)
        {
            return _session.GetJson<List<UserModel>>(UserPath).Where(user => user.Id == id).FirstOrDefault();
        }

        public override UserModel? GetModelByName(string name)
        {
            return _session.GetJson<List<UserModel>>(UserPath).Where(user => user.Name == name).FirstOrDefault(); 
        }
        public override UserModel? GetModelByEmail(string email)
        {
            return _session.GetJson<List<UserModel>>(UserPath).Where(user => user.Email == email).FirstOrDefault();
        }

        public override void CreateUser(UserModel model)
        {
            var UserList = _session.GetJson<List<UserModel>>(UserPath).ToList();
            int maxIndex = UserList.Select(user => user.Id).Max();
            model.Id = maxIndex + 1;
            model.IsActive = false;

            UserList.Add(model);

            _session.SetJson(UserPath, UserList);
            Subjects.userActionSubject.notify(new UserActionMessage() { id = model.Id, actionType = Types.UserActionType.Create });
          
        }

        public override List<UserModel> GetAllUser()
        {
            return _session.GetJson<List<UserModel>>(UserPath).ToList();
        }

        public override void UpdateModel(UserModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            var UserList = _session.GetJson<List<UserModel>>(UserPath).ToList();

            int elements = UserList.RemoveAll(m => m.Id == model.Id);

            if(elements == 0)
            {
                throw new KeyNotFoundException($"User with {model.Id} is not exists in data base");
            }

            UserList.Add(model);
            
            _session.SetJson(UserPath, UserList);
            Subjects.userActionSubject.notify(new UserActionMessage() { id = model.Id, actionType = Types.UserActionType.Update });
        }
    }
}
