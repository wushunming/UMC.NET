
using System;
using System.Collections;
using System.Collections.Generic;
using UMC.Data;
using UMC.Web;
namespace UMC.Demo.Activities
{
    [Mapping("Design", "UI", Auth = WebAuthType.User, Desc = "移动UI设计")]
    public class DesignUIActivity : WebActivity
    {
        bool _isEditer;
        public DesignUIActivity()
        {
            this._isEditer = true;
        }
        public DesignUIActivity(bool isediter)
        {
            _isEditer = isediter;

        }

        void Design(WebRequest request, Guid itemId)
        {

            var entity = Database.Instance().ObjectEntity<Design_Item>();
            entity.Where.And().Equal(new Design_Item { Id = itemId });


            Design_Item item = entity.Single();

            if (item == null)
            {

                Design_Item max = entity
                        .Where.Reset().And().Equal(new Design_Item() { design_id = Guid.Empty, for_id = Guid.Empty })//Design_id(Utility.uuidEmpty).For_id(Utility.uuidEmpty))
                        .Entities.Max(new Design_Item { Seq = 0 });
                max.Seq = (max.Seq ?? 0) + 1;
                item = max;
            }
            Design_Item fitem = item;
            WebMeta meta = this.AsyncDialog(g =>
            {


                UIFormDialog from = new UIFormDialog();
                from.Title = ("页面分类项");

                from.AddText("标题", "ItemName", fitem.ItemName);

                from.AddNumber("顺序", "Seq", fitem.Seq);

                from.Submit("确认", request, "Design");
                return from;
            }, "Setting");
            Design_Item newItem = new Design_Item() { ItemName = meta.Get("ItemName"), ModifiedDate = DateTime.Now, Seq = Utility.Parse(meta.Get("Seq"), 0) };
            entity.Where.Reset().And().Equal(new Design_Item() { Id = itemId });
            entity.IFF(e => e.Update(newItem) == 0, e =>
              {
                  newItem.design_id = Guid.Empty;
                  newItem.for_id = Guid.Empty;
                  newItem.Id = Guid.NewGuid();
                  newItem.Type = UIDesigner.StoreDesignType;
                  e.Insert(newItem);
              });
            this.Context.Send("Design", true);


        }

        void Delete(Guid uuid)
        {
            var entity = Database.Instance().ObjectEntity<Design_Item>();
            entity.Where.And().Equal(new Design_Item { design_id = uuid });
            if (entity.Count() > 0)
            {
                this.Prompt("设计页面有组件，先删除组件，再删除页面项");
            }


            entity.Where.Reset().And().Equal(new Design_Item { Id = uuid });
            entity.Delete();
            this.Context.Send("Design", true);
        }

        public override void ProcessActivity(WebRequest request, WebResponse response)
        {

            Guid designId = UMC.Data.Utility.Guid(this.AsyncDialog("Id", g => this.DialogValue(Guid.Empty.ToString()))).Value;//this.DialogValue(response.Ticket[AttributeNames.TICKET_STORE_ID])), true).Value;


            if (_isEditer)
            {

                WebMeta form = request.SendValues ?? request.Arguments;

                this.AsyncDialog("Model", anycId =>
                {
                    if (form.ContainsKey("limit") == false)
                    {

                        this.Context.Send(new UISectionBuilder(request.Model, request.Command, new WebMeta().Put("Id", designId))
                                .RefreshEvent("Design", "image")
                                .Builder(), true);
                    }
                    var entity = Database.Instance().ObjectEntity<Design_Item>();
                    entity.Where.And().Equal(new Design_Item { design_id = Guid.Empty, for_id = Guid.Empty });
                    entity.Order.Asc(new Design_Item { Seq = 0 });
                    //IObjectEntity<Design_Item> entity = Database.instance().objectEntity(Design_Item.class);
                    //    entity.where().and().equal(new Design_Item().Design_id(Utility.uuidEmpty).For_id(Utility.uuidEmpty));
                    //    entity.order().asc(new Design_Item().Seq(0));
                    Design_Item[] headers = entity.Query();

                    UISection section = UISection.Create(new UITitle("UMC"));

                    int limit = UMC.Data.Utility.Parse(form.Get("limit"), 10);
                    int start = UMC.Data.Utility.Parse(form.Get("start"), 0);

                    switch (headers.Length)
                    {
                        case 0:
                            break;
                        case 1:
                            section.Title.Title = (headers[0].ItemName);
                            //     section.title().right('\ue907', new UIClick("Id", headers[0].Id.toString(), "Model", "none").model(request.model(), request.cmd()));
                            break;
                        default:
                            if (start == 0)
                            {

                                List<WebMeta> items = new List<WebMeta>();
                                foreach (Design_Item item in headers)
                                {

                                    items.Add(new UMC.Web.WebMeta().Put("text", item.ItemName).Put("search", new WebMeta().Put("Id", item.Id)));
                                }
                                section.Add(UICell.Create("TabFixed", new UMC.Web.WebMeta().Put("items", items)));

                            }
                            break;
                    }


                    if (designId == Guid.Empty)
                    {
                        switch (headers.Length)
                        {
                            case 0:
                                break;
                            default:
                                new UIDesigner(true).Section(section, headers[0].Id.Value);
                                break;
                        }
                    }
                    else
                    {
                        new UIDesigner(true).Section(section, designId);
                    }
                    if (section.Length == 0)
                    {

                        section.Add("Desc", new UMC.Web.WebMeta().Put("desc", "未有设计分类项，请添加").Put("icon", "\uEA05")
                            , new UMC.Web.WebMeta().Put("desc", "{icon}\n{desc}"),
                                new UIStyle().Align(1).Color(0xaaa).Padding(20, 20).BgColor(0xfff).Size(12).Name("icon", new UIStyle().Font("wdk").Size(60)));
                    }
                    UIFooter footer = new UIFooter();
                    footer.IsFixed = true;// e);

                    switch (headers.Length)
                    {
                        case 0:

                            footer.AddText(new UIEventText("添加分类项").Click(new UIClick("Model", "News", "Type", "Append").Send(request.Model, request.Command)));
                            break;
                        default:
                            Guid did = designId;
                            if (designId == Guid.Empty)
                            {

                                did = headers[0].Id.Value;

                            }
                            footer.AddIcon(new UIEventText("分类项").Icon('\uf009').Click(new UIClick("Model", "News", "Id", did.ToString()).Send(request.Model, request.Command)));


                            footer.AddText(new UIEventText("增加UI组件").Click(new UIClick(did.ToString()).Send("Design", "Item")));
                            footer.AddText(new UIEventText("查看效果").Style(new UIStyle().BgColor(0xef4f4f))
                                    .Click(new UIClick("Model", "News", "Type", "View").Send(request.Model, request.Command)));


                            break;
                    }


                    section.UIFooter = (footer);
                    response.Redirect(section);
                    return this.DialogValue("none");
                });
                String type = this.AsyncDialog("Type", g =>
                {
                    UIRadioDialog di = new UIRadioDialog();
                    di.Title = ("页面设计");
                    di.Options.Put("编辑分类项", "Edit");
                    di.Options.Put("增加分类项", "Append");
                    di.Options.Put("删除此分类", "Delete");
                    return di;
                });
                switch (type)
                {
                    case "Edit":
                        Design(request, designId);
                        break;
                    case "Append":
                        Design(request, Guid.NewGuid());//.randomUUID());
                        break;
                    case "Delete":
                        Delete(designId);
                        break;
                    case "View":
                        if (request.IsApp)
                        {
                            List<WebMeta> tabs = new List<WebMeta>();

                            Database.Instance().ObjectEntity<Design_Item>()
                                    .Where.And().Equal(new Design_Item() { design_id = Guid.Empty, for_id = Guid.Empty })//(Utility.uuidEmpty).For_id(Utility.uuidEmpty))

                                    .Entities.Order.Asc(new Design_Item() { Seq = 0 })
                                    .Entities.Query(dr =>
                                    {

                                        tabs.Add(new UMC.Web.WebMeta().Put("text", dr.ItemName).Put("search", new UMC.Web.WebMeta().Put("Id", dr.Id.ToString())).Put("cmd", "Home", "model", "Design"));

                                    });
                            if (tabs.Count == 1)
                            {
                                UISectionBuilder builder = new UISectionBuilder("Design", "Home", new WebMeta().Put("Id", tabs[0].GetMeta("search").Get("Id")));
                                //                            builder.builder()
                                this.Context.Send(builder.Builder(), true);//"Tab", new WebMeta().put("sections", tabs).put("text", "UMC界面设计"), true);


                            }
                            else
                            {

                                this.Context.Send("Tab", new WebMeta().Put("sections", tabs).Put("text", "UMC界面设计"), true);

                            }
                        }
                        else
                        {

                            this.AsyncDialog("From", k =>
                            {

                                UIFormDialog fm = new UMC.Web.UIFormDialog();
                                fm.Title = ("移动效果体验");
                                fm.AddImage(new Uri(UMC.Data.Utility.QRUrl("https://oss.365lu.cn/Click/Deisgn/Home/")));


                                fm.AddPrompt("请用支持UMC协议的APP“扫一扫”。");

                                return fm;
                            });
                            break;
                        }
                        break;
                }

            }
            else
            {


                if (designId == Guid.Empty)
                {

                    var entity = Database.Instance().ObjectEntity<Design_Item>();
                    entity.Where.And().Equal(new Design_Item { design_id = Guid.Empty, for_id = Guid.Empty });
                    entity.Order.Asc(new Design_Item { Seq = 0 });

                    List<WebMeta> tabs = new List<WebMeta>();
                    entity.Query(dr =>
                    {
                        tabs.Add(new UMC.Web.WebMeta().Put("text", dr.ItemName).Put("search", new UMC.Web.WebMeta().Put("Id", dr.Id)).Put("cmd", "UI", "model", "Design"));

                    });

                    var chash = new Hashtable();
                    UITitle title = new UITitle("UMC移动界面");
                    title.Left('\uea0e', UIClick.Search());

                    title.Right(new UIEventText().Icon('\uf2c0').Click(new UIClick().Send("Account", "Info")));


                    chash.Add("sections", tabs);
                    chash.Add("title", title);
                    response.Redirect(chash);

                }
                else
                {
                    UIDesigner designer = new UIDesigner(false);
                    response.Redirect(designer.Section("", designId));

                }
            }

        }

    }

}