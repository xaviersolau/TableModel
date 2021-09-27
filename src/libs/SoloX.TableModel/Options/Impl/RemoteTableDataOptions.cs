using System;
using SoloX.TableModel.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SoloX.TableModel.Impl;

namespace SoloX.TableModel.Options.Impl
{
    public class RemoteTableDataOptions<TData> : ATableDataOptions, IRemoteTableDataOptions<TData>
    {
        public RemoteTableDataOptions(string tableId)
            : base(tableId)
        {
        }

        public HttpClient HttpClient { get; set; }
        public override Task<ITableData> CreateModelInstanceAsync(IServiceProvider serviceProvider)
        {
            return Task.FromResult<ITableData>(new RemoteTableData<TData>(TableDataId, HttpClient));
        }
    }
}
