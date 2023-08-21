﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext.Interfaces
{
    /// <summary>
    /// не много спорное решение, нужна ли абстракция в данном коде?
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(Guid itemId);
        Task Save();
    }
}
