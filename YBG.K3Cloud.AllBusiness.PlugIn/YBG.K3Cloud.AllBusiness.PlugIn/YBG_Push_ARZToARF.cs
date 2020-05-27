﻿using Kingdee.BOS.Contracts;
using Kingdee.BOS.Core.Metadata;
using Kingdee.BOS.Core.Metadata.ConvertElement.PlugIn;
using Kingdee.BOS.Core.Metadata.ConvertElement.PlugIn.Args;
using Kingdee.BOS.Core.Metadata.FieldElement;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.ServiceHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YBG.K3Cloud.AllBusiness.PlugIn
{
    [Description("暂估应收下推财务应收的价格控制")]
    [Kingdee.BOS.Util.HotUpdate]
    public class YBG_Push_ARZToARF : AbstractConvertPlugIn
    {
        ///<summary>
        ///获取源单数据源
        ///</summary>
        public override void OnGetSourceData(GetSourceDataEventArgs e)
        {
            e.SourceData = GetSplitSourceData(e.SourceBusinessInfo, e.SourceData);

        }
        ///<summary>
        ///修改含税单价
        ///</summary>
        private DynamicObjectCollection GetSplitSourceData(BusinessInfo billInfo,DynamicObjectCollection sourceData)
        {
           // DynamicObjectCollection newSourceData = new DynamicObjectCollection(sourceData.DynamicCollectionItemPropertyType);
            foreach (var oneSourceData in sourceData)
            {
                string FentrtyID = oneSourceData["FEntityDetail_FEntryID"].ToString();

                string sql = string.Format(@"select FARFTAXPRICE from T_SAL_OUTSTOCKENTRY  
                                            a left join  T_AR_RECEIVABLEENTRY_LK b on a.FENTRYID=b.FSID where b.FENTRYID='{0}'", FentrtyID);
                //含税单价
                decimal FARFTAXPRICE = DBServiceHelper.ExecuteScalar<decimal>(this.Context, sql, 0, null);
                if (FARFTAXPRICE > 0)
                {
                    oneSourceData["FTaxPrice"] = FARFTAXPRICE;
                }
            }
            return sourceData;
        }
    }
}

