﻿using System;
using System.Data.Entity;
﻿using Filewatcher.MDL;

namespace Filewatcher.DAL
{
    public interface IRepositoryProvider
    {
        DbContext DbContext { get; set; }
        IRepository<T> GetRepositoryForEntityType<T>() where T : class, IEntity;
        T GetRepository<T>(Func<DbContext, object> factory = null) where T : class;
        void SetRepository<T>(T repository);
    }
}
