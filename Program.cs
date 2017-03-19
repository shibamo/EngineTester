using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using System.Reflection;
using System.Dynamic;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Scripting;

using EnouFlowTemplateLib;
using EnouFlowOrgMgmtLib;
using EnouFlowEngine;
using EnouFlowInstanceLib;
using OPAS2Model;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace EngineTester
{
  class Program
  {
    static void Main(string[] args)
    {

      bool testCreateOrgStru = false;
      bool testQueryOrgStru = false;
      bool testCreateSystemManager = false;

      bool testImportFlowDefinitionJSON = false;
      bool testCreateFlowTemplate = false;

      // Actions
      bool testCreateFlowActionStart = false;
      bool testCreateFlowActionMoveTo_1 = false;
      bool testCreateFlowActionMoveTo_2 = false;
      bool testCreateFlowActionMoveTo_3 = false;
      bool testCreateFlowActionMoveTo_4 = false;
      bool testCreateFlowActionRejectToStart = false;
      bool testCreateFlowActionJumpTo_1 = false;
      //bool testCreateFlowActionTake = false;
      //bool testCreateFlowActionInviteOther = false;
      //bool testCreateFlowActionFeedBackOfInvite = false;

      // Engine
      bool testRunEngine = false;

      // OPAS2Model
      bool testBizDocumentSerialNoGenerator = false;
      bool testCreatePR = false;
      bool testGenerateBizDataPayloadJson = false;

      // FlowDynamicUser
      bool testFlowDynamicUser = true;

      #region Test OrgMgmtLib

      #region Creation
      if (testCreateOrgStru)
      {
        using (var db = new EnouFlowOrgMgmtContext())
        {
          // var DBHelper = DBHelper;

          var _org = OrgMgmtDBHelper.createOrg(db);
          _org.name = "CQAC集团";
          OrgMgmtDBHelper.saveCreatedOrg(_org, db);

          var _orgSchema = OrgMgmtDBHelper.createOrgSchema(db);
          _orgSchema.name = "默认组织结构方案";
          OrgMgmtDBHelper.saveCreatedOrgSchema(_org, _orgSchema, db);

          var _bizEntity = OrgMgmtDBHelper.createBizEntity(db);
          _bizEntity.name = "Qoros汽车有限公司";
          OrgMgmtDBHelper.saveCreatedBizEntity(_orgSchema, _bizEntity, null, db);

          //var _bizEntityChild = OrgMgmtDBHelper.createBizEntity(db);
          //_bizEntityChild.name = "上海总部";
          //OrgMgmtDBHelper.saveCreatedBizEntity(_orgSchema, _bizEntityChild, _bizEntity, db);

          //var _bizEntityChild2 = OrgMgmtDBHelper.createBizEntity(db);
          //_bizEntityChild2.name = "常熟生产基地";
          //OrgMgmtDBHelper.saveCreatedBizEntity(_orgSchema, _bizEntityChild2, _bizEntity, db);

          var _bizEntitySchema = OrgMgmtDBHelper.createBizEntitySchema(db);
          _bizEntitySchema.name = "Qoros默认公司架构";
          OrgMgmtDBHelper.saveCreatedBizEntitySchema(_bizEntitySchema, _bizEntity, db);

          var _department0 = OrgMgmtDBHelper.createDepartment(db);
          _department0.name = "董事会";
          OrgMgmtDBHelper.saveCreatedDepartment(_bizEntitySchema, _department0, null, db);

          //var _department = OrgMgmtDBHelper.createDepartment(db) ;
          //_department.name = "后台部门";
          //OrgMgmtDBHelper.saveCreatedDepartment(_bizEntitySchema, _department, null, db);

          var _department1 = OrgMgmtDBHelper.createDepartment(db);
          _department1.name = "产品部";
          OrgMgmtDBHelper.saveCreatedDepartment(_bizEntitySchema, _department1, null, db);

          var _departmentChild = OrgMgmtDBHelper.createDepartment(db);
          _departmentChild.name = "财务部";
          OrgMgmtDBHelper.saveCreatedDepartment(_bizEntitySchema,
            _departmentChild, null, db);

          var _user = OrgMgmtDBHelper.createUser(db);
          _user.name = "张三";
          OrgMgmtDBHelper.saveCreatedUser(_user, db);

          var _user2 = OrgMgmtDBHelper.createUser(db);
          _user2.name = "李四";
          OrgMgmtDBHelper.saveCreatedUser(_user2, db);

          var _user3 = OrgMgmtDBHelper.createUser(db);
          _user3.name = "王五";
          OrgMgmtDBHelper.saveCreatedUser(_user3, db);

          OrgMgmtDBHelper.saveDepartmentUserRelation(_departmentChild, _user, db);
          OrgMgmtDBHelper.saveDepartmentUserRelation(_departmentChild, _user2, db);
          OrgMgmtDBHelper.saveDepartmentUserRelation(_department1, _user3, db);

          var _role = OrgMgmtDBHelper.createRole(db);
          _role.name = "系统管理员";
          OrgMgmtDBHelper.saveCreatedRole(_role, db);

          var _role1 = OrgMgmtDBHelper.createRole(db);
          _role1.name = "CXOs";
          OrgMgmtDBHelper.saveCreatedRole(_role1, db);

          OrgMgmtDBHelper.saveCreatedRoleUserRelation(_role, _user, db);
          OrgMgmtDBHelper.saveCreatedRoleUserRelation(_role1, _user, db);

          OrgMgmtDBHelper.saveCreatedRoleUserRelation(_role1, _user2, db);

          var _roleType = OrgMgmtDBHelper.createRoleType(db);
          _roleType.name = "系统";
          OrgMgmtDBHelper.saveCreatedRoleType(_roleType, db);

          OrgMgmtDBHelper.saveCreatedRole_RoleTypeRelation(_role, _roleType, db);
        }
      }
      #endregion

      #region Query
      if (testQueryOrgStru)
      {
        using (var db = new EnouFlowOrgMgmtContext())
        {

          //找到一个OrgSchema下的顶级业务实体集
          var defaultOrg = db.orgs.ToList().FirstOrDefault();
          if (defaultOrg == null)
          {
            Console.WriteLine("No defaultOrg Exists.");
            Console.Read();
            return;
          }

          var defaultOrgSchema = defaultOrg.orgSchema;
          if (defaultOrgSchema == null)
          {
            Console.WriteLine("No defaultOrgSchema Exists.");
            Console.Read();
            return;
          }

          var rootBizEntities = defaultOrg.orgSchema.rootBizEntities;

          var rootBizEntity = rootBizEntities[0];
          if (rootBizEntity == null)
          {
            Console.WriteLine("No rootBizEntity Exists.");
            Console.Read();
            return;
          }

          //找到一个业务实体指定OrgSchema的下级业务实体集
          var bizEntityChildren = rootBizEntity.getBizEntitiesChildren(
            db, db.orgs.ToList().FirstOrDefault().orgSchema);

          var bizEntityChild = bizEntityChildren[0];

          //找到一个业务实体指定OrgSchema的上级业务实体
          var bizEntityParent = bizEntityChild.getBizEntitiyParent(
            db, db.orgs.ToList().FirstOrDefault().orgSchema);

          //找到一个业务实体指定BizEntitySchema下的顶级部门集
          var depts = bizEntityChild.bizEntitySchemas[0].getRootDepartments(db);

          //找到一个部门的所有下级部门
          var dps = depts.LastOrDefault().getDepartmentChildren(db);

          //找到一个部门的上级部门
          var dp1 = depts.LastOrDefault().getDepartmentChildren(db)[1];

          var dpParent = dp1.getParentDepartment(db);
          dpParent = dpParent.getParentDepartment(db);

          //找到一个部门的所有用户
          var usrs = dp1.getUserChildren(db);

          //找到一个用户的所有部门
          var u = usrs[0];
          var depts1 = u.getDepartmentsBelongTo(db);

          //找到一个用户的所有角色
          var roles = u.getRolesBelongTo(db);

          //找到一个角色的所有用户
          var cxos = db.roles.ToList()[1].getUsersBelongTo(db);

          Console.WriteLine("查询完成.");
          //return;
        }
      }
      #endregion

      #endregion

      #region Test OrgMgmtLib.SystemManager

      if (testCreateSystemManager)
      {
        using (var db = new EnouFlowOrgMgmtContext())
        {
          //if (db.systemManagers.Count() <= 0)
          //{
          //  var _sysmgr = db.systemManagers.Create();
          //  _sysmgr.name = "系统管理员";
          //  _sysmgr.logonName = "sys";
          //  _sysmgr.logonSalt = "f6152039-1745-4f4c-8f8e-3ed37770fa0d";
          //  _sysmgr.logonPasswordHash = Convert.ToBase64String(
          //    new System.Security.Cryptography.SHA256Managed().ComputeHash(
          //      Encoding.UTF8.GetBytes(
          //        "111111" + //测试密码
          //        "f6152039-1745-4f4c-8f8e-3ed37770fa0d")));

          //  db.systemManagers.Add(_sysmgr);
          //  db.SaveChanges();
          //  Console.WriteLine("成功创建系统管理员");
          //}
        }
      }

      #endregion

      var PRFlowTemplateFilePath = @"C:\data\MyProjs\99Flow\EnouFlow\internal\test_flow.json";
      var POFlowTemplateFilePath = @"C:\data\MyProjs\99Flow\EnouFlow\EnouFlowEngine\PO.json";
      var GRFlowTemplateFilePath = @"C:\data\MyProjs\99Flow\EnouFlow\EnouFlowEngine\GR.json";
      var PMFlowTemplateFilePath = @"C:\data\MyProjs\99Flow\EnouFlow\EnouFlowEngine\PM.json";

      #region Test import Flow Definition JSON
      if (testImportFlowDefinitionJSON)
      {
        StreamReader oJsonFile = new StreamReader(
          POFlowTemplateFilePath);

        //StreamReader oJsonFile = new System.IO.StreamReader(
        //@"C:\data\MyProjs\99Flow\EnouFlow\EnouFlowEngine\test_flow.json");

        var flowTemplateDefTest = JsonHelper.DeserializeJsonToObject<FlowTemplateDef>(
          oJsonFile.ReadToEnd());

        dynamic temp = parseJsonToDynamicObject("{'AmountTotal': 49999}");

        Console.WriteLine("成功导入流程定义文件");
      }
      #endregion

      #region Test Create Flow Template in DB
      if (testCreateFlowTemplate)
      {
        StreamReader oJsonFile = new StreamReader(PMFlowTemplateFilePath);

        var tplJson = oJsonFile.ReadToEnd();
        var flowTemplateDefTest = JsonHelper.DeserializeJsonToObject<FlowTemplateDef>(
          tplJson);

        var tpl = FlowTemplateDBHelper.createFlowTemplate();

        tpl.guid = flowTemplateDefTest.basicInfo.guid;
        tpl.name = flowTemplateDefTest.basicInfo.name;
        tpl.displayName = flowTemplateDefTest.basicInfo.displayName;
        tpl.version = flowTemplateDefTest.basicInfo.version;
        tpl.code = "PM";
        tpl.flowTemplateJson = tplJson;

        FlowTemplateDBHelper.saveCreatedFlowTemplate(tpl);
        Console.WriteLine("成功创建流程定义模板");

      }
      #endregion

      #region Test Create FlowActions

      #region Test Create FlowActionStart
      if (testCreateFlowActionStart)
      {
        var actionStart = FlowInstanceHelper.PostFlowActionStart(
          "aaaaaaa-1745-4f4c-8f8e-3ed37770fa0d",
          "bizDocumentGuid", "PR",
          "用户输入的备注内容aaa", null, null, 1,
          "abc", 1, "def", null, "TestFlow-001",
          "f6152039-1745-4f4c-8f8e-3ed37770fa0d"
        );
      }
      #endregion

      #region Test Create FlowActionMoveTo
      if (testCreateFlowActionMoveTo_1)
      {
        var action1 = FlowInstanceHelper.PostFlowActionMoveTo(
          "bbbbbbb-1745-4f4c-8f8e-3ed37770fa0d",
          "bizDocumentGuid", "PR",
          DateTime.Now.AddSeconds(1), "用户输入的备注内容bbb",
          "{'AmountTotal': 50001}",
          "{'AmountTotalUpdated': 49999}", 1,
          "user-guid", 0, null, "TestFlow-001",
          "f6152039-1745-4f4c-8f8e-3ed37770fa0d",
          "9d985ac5-a1da-4c63-8aea-83e9974ffccc",
          "9d4a6006-1099-46ad-b037-24e84476ab50",
          new List<Paticipant>() {
            new Paticipant("user",
              new PaticipantDigest(
              "李四",
              "27bcd361-12c7-4376-8dd8-ce68ad964431",
              2,null,null)
              )
          }
        );
      }

      if (testCreateFlowActionMoveTo_2)
      {
        var action2 = FlowInstanceHelper.PostFlowActionMoveTo(
          "cccccc-1745-4f4c-8f8e-3ed37770fa0d",
          "bizDocumentGuid", "PR",
          DateTime.Now.AddSeconds(2),
          "用户输入的备注内容ccc", null, null, 1,
          "user-guid", 1, "flow-inst-guid", "TestFlow-001",
          "2dde0ed2-c10b-4c98-b263-1016dcfa951d",
          "55d72a3e-051a-46a0-9aae-0be1c62b2e24",
          "ab0f92f6-35cd-44b9-a7bd-5a17e544aa9c",
          new List<Paticipant>() {
            new Paticipant("user",
              new PaticipantDigest(
              "王五",
              "f17004ea-1246-40df-9f48-2adf0cfa8517",
              3,null,null)
              )
          }
        );
      }

      if (testCreateFlowActionMoveTo_3)
      {
        var action3 = FlowInstanceHelper.PostFlowActionMoveTo(
          "dddddd-1745-4f4c-8f8e-3ed37770fa0d",
          "bizDocumentGuid", "PR",
          DateTime.Now.AddSeconds(3),
          "用户输入的备注内容ddd", null, null, 1,
          "user-guid", 1, "flow-inst-guid", "TestFlow-001",
          "ab0f92f6-35cd-44b9-a7bd-5a17e544aa9c",
          "ceeae036-7b21-438f-bc46-33bfad0cc546",
          "5ee19a5b-df8b-4c11-a0d4-082848b5f216",
          new List<Paticipant>() {
            new Paticipant("user",
              new PaticipantDigest(
              "张三",
              "85a14f4d-e68e-4684-88b2-3bf9dea386e6",
              1,null,null)
              )
          }
        );
      }

      if (testCreateFlowActionMoveTo_4)
      {
        var action4 = FlowInstanceHelper.PostFlowActionMoveTo(
          "eeeeee-1745-4f4c-8f8e-3ed37770fa0d",
          "bizDocumentGuid", "PR",
          DateTime.Now.AddSeconds(4),
          "用户输入的备注内容eee", null, null, 1,
          "user-guid", 1, "flow-inst-guid", "TestFlow-001",
          "5ee19a5b-df8b-4c11-a0d4-082848b5f216",
          "0dade9b8-acc9-4223-8dab-9d55480722a8",
          "c09fcfbe-92a9-48cd-bd14-975996e063a7",
          new List<Paticipant>()
        //{
        //  new Paticipant("user",
        //    new PaticipantDigest(
        //    "川普",
        //    "c4961686-41a6-469a-afa9-df05e42ba9f8",
        //    16,null)
        //    )
        //}
        );
      }

      #endregion 

      #region TODO: Test Create FlowActionTake

      #endregion 

      #region TODO: Test Create FlowActionInviteOther
      #endregion 

      #region TODO: Test Create FlowActionFeedBackOfInvite
      #endregion 

      #endregion

      #region Test Engine
      if (testRunEngine)
      {
        var dispatcher = new FlowActionRequestDispatcher();
        //EnumFlowActionRequestType[] types = new EnumFlowActionRequestType[] { EnumFlowActionRequestType.moveToAutoGenerated };
        //dispatcher.processNextActionOfSpecifiedInstance(1, types);
        var result = dispatcher.processNextAction();
      }

      #endregion

      #region Test BizDocumentSerialNoGenerator
      if (testBizDocumentSerialNoGenerator)
      {
        var prNumberNew = new OPAS2ModelDBHelper().
          generateDocumentSerialNo(EnumBizDocumentType.PR, "QOROS", "SH", "2017", "IT");
        Console.WriteLine(prNumberNew);
      }

      #endregion

      #region Test CreatePR
      if (testCreatePR)
      {
        var prNumberNew = new OPAS2ModelDBHelper().
          generateDocumentSerialNo(EnumBizDocumentType.PR, "QOROS", "SH", "2017", "IT");
        Console.WriteLine(prNumberNew);
        using (var db = new OPAS2DbContext())
        {
          var pr = db.purchaseReqs.Create();
          pr.guid = "test-pr-guid:" + Guid.NewGuid().ToString();
          pr.documentNo = new OPAS2ModelDBHelper().generateDocumentSerialNo(
            EnumBizDocumentType.PR, "QOROS", "SH", "2017", "IT");
          pr.WBSNo = "test-WBS";
          pr.contactOfficePhone = "contactOfficePhone";
          pr.contactMobile = "contactMobile";
          pr.contactOtherMedia = "contactOtherMedia";
          pr.departmentId = 1;
          pr.departmentIdBelongTo = 1;
          pr.costCenterId = 1;
          pr.expectReceiveBeginTime = DateTime.Now;
          pr.expectReceiveEndTime = DateTime.Now;
          pr.isBidingRequired = true;
          pr.noBiddingReason = "noBiddingReason";
          pr.reason = "reason";
          pr.description = "description";
          pr.estimatedCost = (decimal)123456.789;
          pr.currencyTypeId = 1;
          pr.mainCurrencyRate = (decimal)1.0;
          pr.estimatedCostInRMB = (decimal)123456.789;
          pr.averageBenchmark = (decimal)787654.78;
          pr.benchmarkDescription = "benchmarkDescription";
          pr.isFirstBuy = false;
          pr.firstBuyDate = DateTime.Now;
          pr.firstCostAmount = (decimal)73663387.87;
          pr.firstBuyDescription = "firstBuyDescription";
          pr.remarkOfAprrovers = "remarkOfAprrovers";
          pr.otherVendorsNotInList = "otherVendorsNotInList";
          pr.submitTime = DateTime.Now;
          pr.submitor = "ChaoQin";
          pr.submitorUserId = 1;
          pr.creator = "ChaoQin";
          pr.creatorUserId = 1;

          var prDtl1 = db.purchaseReqDetails.Create();
          prDtl1.PurchaseReq = pr;
          prDtl1.estimatedCost = (decimal)123.45;
          prDtl1.lineNo = 5;
          prDtl1.itemName = "name1";
          prDtl1.itemType = (EnumPRItemType)1;
          prDtl1.description = "description1";
          prDtl1.creator = "ChaoQin";
          prDtl1.creatorUserId = 1;

          //pr.details.Add(prDtl1);

          db.purchaseReqs.Add(pr);
          db.purchaseReqDetails.Add(prDtl1);

          db.SaveChanges();

          Console.WriteLine("PR: " + pr.purchaseReqId.ToString());
        }
      }

      #endregion

      #region Test GenerateBizDataPayloadJson
      if (testGenerateBizDataPayloadJson)
      {
        using (var db = new OPAS2DbContext())
        {
          var pr = db.purchaseReqs.Find(8);
          dynamic bizDataPayload = new ExpandoObject();
          bizDataPayload.document = pr;
          //bizDataPayload.subDocuments = pr.details;
          bizDataPayload.AmountTotal =
            pr.details.Aggregate<PurchaseReqDetail, decimal>(
              0, (total, detail) =>
              {
                return total + detail.estimatedCost.Value;
              });
          var s = JsonConvert.SerializeObject(bizDataPayload, Formatting.Indented,
            new JsonSerializerSettings
            {
              ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            }
          );
          Console.WriteLine(s);
        }
      }
      #endregion

      #region testFlowDynamicUser
      Console.Out.WriteLine("Enter into script:");
      var _session = new DictionaryContext();
      var flowInstDb = new EnouFlowInstanceContext();
      var flowInstance = FlowInstanceHelper.GetFlowInstance(42, flowInstDb);

      _session.globals.Add("flowInstance", flowInstance);
      var _references = new Assembly[] { typeof(DictionaryContext).Assembly,
                             typeof(System.Runtime.CompilerServices.DynamicAttribute).Assembly,
                             typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo).Assembly,
                             typeof(ExpandoObject).Assembly,
                             typeof(JsonConvert).Assembly,
                             typeof(ExpandoObjectConverter).Assembly,
                             typeof(Paticipant).Assembly,
                             typeof(UserDTO).Assembly,
                             typeof(FlowInstance).Assembly,
                             typeof(List<>).Assembly
      };
      var _imports = new string[] {typeof(DictionaryContext).Namespace,
                          typeof(System.Runtime.CompilerServices.DynamicAttribute).Namespace,
                          typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo).Namespace,
                          typeof(ExpandoObject).Namespace,
                          typeof(JsonConvert).Namespace,
                          typeof(ExpandoObjectConverter).Namespace,
                          typeof(Paticipant).Namespace,
                          typeof(UserDTO).Namespace,
                          typeof(FlowInstance).Namespace,
                          typeof(List<>).Namespace
      };
      var _options = ScriptOptions.Default
        .AddReferences(_references)
        .AddImports(_imports);
      var code = @"var result = new List<UserDTO>(); 
                    result.Add(OrgMgmtDBHelper.getUserDTO(((FlowInstance)globals[""flowInstance""]).creatorId));
                    return result;";
      var result1 = CSharpScript.RunAsync(code, 
        globals: _session, options: _options).Result.ReturnValue;
      var retList = (List<UserDTO>) result1;

      #endregion

      Console.WriteLine("All done!");
      Console.Read();
    }

    private static dynamic parseJsonToDynamicObject(string json)
    {
      if (string.IsNullOrWhiteSpace(json)) return null;

      dynamic temp = JsonConvert.DeserializeObject<ExpandoObject>(
          json, new ExpandoObjectConverter());
      return temp;
    }
  }
}
