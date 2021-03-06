﻿using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DemoLibrary
{
    public class Demo3
    {
        public interface ITransient { }
        public interface IUser : ITransient
        {
            string GetName();
        }
        public class User : IUser
        {
            public string GetName()
            {
                return "农码一生";
            }
        }

        public class AssmInstaller : IWindsorInstaller
        {
            public void Install(IWindsorContainer container, IConfigurationStore store)
            {
                container.Register(Classes.FromThisAssembly()   //选择Assembly
                    .IncludeNonPublicTypes()                    //约束Type
                    .BasedOn<ITransient>()                      //约束Type
                    .WithService.DefaultInterfaces()            //匹配类型
                    .LifestyleTransient());                     //注册生命周期
            }
        }


        public class UserService : ITransient
        {
            private readonly IUser _user;
            public UserService(IUser user)
            {
                _user = user;
            }

            public string GetUserName()
            {
                return "UserService:" + _user.GetName();
            }
        }
        public class User_Tests
        {
            [Fact]
            public void GetName_IWindsorInstaller_Ok()
            {
                var container = new WindsorContainer();
                container.Install(FromAssembly.This());
                var user = container.Resolve<IUser>();
                var name = user.GetName();
                Assert.True(name == "农码一生");

                var userService = container.Resolve<UserService>();//注入框架 自动给UserService的构造函数注入了IUser
                var str = userService.GetUserName();
                Assert.True(str == "UserService:农码一生");
            }
        }
    }
}
