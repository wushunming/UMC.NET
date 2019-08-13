

using System;
using UMC.Data;
using UMC.Web;
namespace UMC.Demo.Activities
{

    class DesignItemActivity : WebActivity
    {
        void Seq(WebRequest request, WebResponse response, Design_Item item)
        {
            var entity = Database.Instance().ObjectEntity<Design_Item>();
            entity.Where.And().Equal(new Design_Item() { Id = (item.Id) });

            WebMeta meta = this.AsyncDialog(g =>
            {

                UIFormDialog from = new UIFormDialog();
                from.Title = ("调整顺序");

                from.AddNumber("展示顺序", "Seq", item.Seq);
                return from;
            }, "Setting");


            entity.Update(new Design_Item() { ModifiedDate = DateTime.Now, Seq = Utility.IntParse(meta.Get("Seq"), 0) });


        }

        void Caption(WebRequest request, WebResponse response, Guid sid, Guid forid)
        {
            String Name = this.AsyncDialog("Name", g => new UITextDialog() { Title = ("新建栏位") });
            Design_Item item2 = new Design_Item()
            {
                Id = Guid.NewGuid(),
                design_id = sid,
                for_id = forid,
                ModifiedDate = DateTime.Now,
                Type = UIDesigner.StoreDesignTypeCaption,
                ItemName = Name
            };
            var entity = Database.Instance().ObjectEntity<Design_Item>();

            Design_Item max = entity
                    .Where.And().Equal(new Design_Item() { design_id = (sid) })
                    .Entities.Max(new Design_Item() { Seq = 0 });//.Seq+1;
            item2.Seq = (max.Seq ?? 0) + 1;

            entity.Insert(item2);
        }


        void Icons(WebRequest request, Guid itemId)
        {

            var entity = Database.Instance().ObjectEntity<Design_Item>();

            entity.Where.And().Equal(new Design_Item() { Id = itemId });
            Design_Item item = entity.Single();

            var webr = UMC.Data.WebResource.Instance();

            WebMeta meta = this.AsyncDialog(g =>
            {
                Design_Item finalItem = item;
                switch (item.Type ?? 0)
                {
                    case UIDesigner.StoreDesignTypeItem:
                        break;
                    case UIDesigner.StoreDesignTypeIcons:
                        Design_Item item2 = new Design_Item()
                        {
                            Id = Guid.NewGuid(),
                            design_id = item.design_id,
                            for_id = item.Id,
                            ModifiedDate = DateTime.Now,
                            Type = UIDesigner.StoreDesignTypeItem
                        };

                        Design_Item max = entity
                      .Where.And().Equal(new Design_Item() { design_id = item.design_id, for_id = item.Id })
                      .Entities.Max(new Design_Item() { Seq = 0 });//.Seq+1;
                        item2.Seq = (max.Seq ?? 0) + 1;

                        entity.Insert(item2);



                        finalItem = item2;
                        request.Arguments.Put("Id", finalItem.Id);
                        break;
                    default:
                        this.Prompt("类型错误");
                        break;
                }


                UIFormDialog from = new UIFormDialog();
                from.Title = ("图标");

                from.AddFile("图片", "_Image", webr.ImageResolve(finalItem.Id.Value, "1", 4))
                        .Command("Design", "Image", new UMC.Web.WebMeta().Put("id", finalItem.Id).Put("seq", "1"));
                from.AddText("标题", "ItemName", finalItem.ItemName);

                from.AddNumber("顺序", "Seq", finalItem.Seq);

                from.Submit("确认", request, "Design");
                return from;
            }, "Setting");


            entity.Where.Reset().And().Equal(new Design_Item() { Id = item.Id });
            entity.Update(new Design_Item() { ItemName = meta.Get("ItemName"), ModifiedDate = DateTime.Now, Seq = Utility.IntParse(meta.Get("Seq"), 0) });


        }

        void Items(WebRequest request, Guid itemId)
        {
            UMC.Data.WebResource webr = UMC.Data.WebResource.Instance();

            var entity = Database.Instance().ObjectEntity<Design_Item>();
            entity.Where.Reset().And().Equal(new Design_Item() { Id = itemId });

            Design_Item item = entity.Single();


            WebMeta meta = this.AsyncDialog(g =>
            {
                Design_Item finalItem = item;
                switch (item.Type)
                {
                    case UIDesigner.StoreDesignTypeItem:
                        break;
                    case UIDesigner.StoreDesignTypeItems:
                        int count = entity.Where.Reset()
                                .And().Equal(new Design_Item() { for_id = itemId })
                                .Entities.Count();
                        if (count > 3)
                        {
                            this.Prompt("分列栏，只能添加4列");
                        }

                        Design_Item item2 = new Design_Item()
                        {
                            Id = Guid.NewGuid(),
                            design_id = item.design_id,
                            for_id = item.Id,
                            ModifiedDate = DateTime.Now,
                            Type = UIDesigner.StoreDesignTypeItem
                        };

                        Design_Item max = entity
                      .Where.And().Equal(new Design_Item() { design_id = item.design_id, for_id = item.Id })
                      .Entities.Max(new Design_Item() { Seq = 0 });//.Seq+1;
                        item2.Seq = (max.Seq ?? 0) + 1;

                        entity.Insert(item2);



                        finalItem = item2;
                        request.Arguments.Put("Id", finalItem.Id);

                        break;
                    default:
                        this.Prompt("类型错误");
                        break;
                }

                WebMeta data = UMC.Data.JSON.Deserialize<WebMeta>(finalItem.Data) ?? new UMC.Web.WebMeta();

                UIFormDialog from = new UIFormDialog();
                from.Title = ("图标");
                from.AddFile("图片", "_Image", webr.ImageResolve(finalItem.Id.Value, "1", 4))
                        .Command("Design", "Image", new UMC.Web.WebMeta().Put("id", finalItem.Id).Put("seq", "1"));
                from.AddText("标题", "title", finalItem.ItemName);
                from.AddText("描述", "desc", finalItem.ItemDesc);
                from.Add("Color", "startColor", "标题开始色", data.Get("startColor"));
                from.Add("Color", "endColor", "标题结束色", data.Get("endColor"));
                from.AddNumber("顺序", "Seq", finalItem.Seq);
                // from.submit("确认", request.model(), request.cmd(), new UMC.Web.WebMeta().put("Id", finalItem.Id).put("Type", "Edit"));

                from.Submit("确认", request, "Design");
                return from;
            }, "Setting");

            entity.Where.Reset().And().Equal(new Design_Item() { Id = item.Id });
            entity.Update(new Design_Item()
            {
                ItemName = meta.Get("title"),
                ItemDesc = meta.Get("desc"),
                Data = UMC.Data.JSON.Serialize(meta),
                ModifiedDate = DateTime.Now,
                Seq = Utility.IntParse(meta.Get("Seq"), 0)
            });

            //entity.Where.reset().And().Equal(new Design_Item().Id(item.Id));
            //entity.update(new Design_Item().ItemName(meta.get("title"))
            //        .ItemDesc(meta.get("desc"))
            //        .Data(UMC.Data.JSON.serialize(meta))
            //        .ModifiedDate(new Date()).Seq(Utility.parse(meta.get("Seq"), 0)));


        }


        void TitleDesc(WebRequest request, Guid itemId)
        {
            UMC.Data.WebResource webr = UMC.Data.WebResource.Instance();

            var entity = Database.Instance().ObjectEntity<Design_Item>();
            entity.Where.Reset().And().Equal(new Design_Item() { Id = itemId });

            Design_Item item = entity.Single();


            WebMeta meta = this.AsyncDialog(g =>
            {
                WebMeta config = new UMC.Web.WebMeta();
                switch (item.Type ?? 0)
                {
                    case UIDesigner.StoreDesignTypeItem:
                        Design_Item parent = entity.Where.Reset().And().Equal(new Design_Item() { Id = item.for_id }).Entities.Single();


                        config = UMC.Data.JSON.Deserialize<WebMeta>(parent.Data) ?? new UMC.Web.WebMeta();

                        break;

                    case UIDesigner.StoreDesignTypeTitleDesc:
                        config = UMC.Data.JSON.Deserialize<WebMeta>(item.Data) ?? new UMC.Web.WebMeta();

                        Design_Item item2 = new Design_Item()
                        {
                            Id = Guid.NewGuid(),
                            design_id = item.design_id,
                            for_id = item.Id,
                            ModifiedDate = DateTime.Now,
                            Type = UIDesigner.StoreDesignTypeItem
                        };

                        Design_Item max = entity
                      .Where.And().Equal(new Design_Item() { design_id = item.design_id, for_id = item.Id })
                      .Entities.Max(new Design_Item() { Seq = 0 });//.Seq+1;
                        item2.Seq = (max.Seq ?? 0) + 1;

                        entity.Insert(item2);



                        item = item2;
                        request.Arguments.Put("Id", item.Id);
                        break;
                    default:
                        this.Prompt("类型错误");
                        break;
                }

                //config = UMC.Data.JSON.Deserialize<WebMeta>(item.Data) ?? new UMC.Web.WebMeta();


                WebMeta data = UMC.Data.JSON.Deserialize<WebMeta>(item.Data) ?? new UMC.Web.WebMeta();
                ;// Utility.isNull(UMC.Data.JSON.deserialize(finalItem.Data, WebMeta.class), new UMC.Web.WebMeta());

                UIFormDialog from = new UIFormDialog();
                from.Title = ("图文项");


                String total = data.Get("Total") ?? "1";

                from.AddFile(String.Format("{0}比例图片", total == "1" ? "100:55" : "1:1"), "_Image",
                                webr.ImageResolve(item.Id.Value, "1", 4))
                                .Command("Design", "Image", new UMC.Web.WebMeta().Put("id", item.Id).Put("seq", "1"));
                String hide = config.Get("Hide") ?? "";
                if (hide.IndexOf("HideTitle") == -1)
                    from.AddText("图文标题", "title", item.ItemName);
                if (hide.IndexOf("HideDesc") == -1)
                    from.AddText("图文描述", "desc", item.ItemDesc);
                if (hide.IndexOf("HideLeft") == -1)
                    from.AddText("左角价格", "left", data.Get("left"));
                if (hide.IndexOf("HideRight") == -1)
                    from.AddText("右角说明", "right", data.Get("right"));
                from.AddNumber("顺序", "Seq", item.Seq);

                from.Submit("确认", request, "Design");
                return from;
            }, "Setting");

            entity.Where.Reset().And().Equal(new Design_Item() { Id = item.Id });
            entity.Update(new Design_Item()
            {
                ItemName = meta.Get("title"),
                ItemDesc = meta.Get("desc"),
                Data = UMC.Data.JSON.Serialize(meta),
                ModifiedDate = DateTime.Now,
                Seq = Utility.IntParse(meta.Get("Seq"), 0)
            });


            //entity.Where.reset().And().Equal(new Design_Item().Id(item.Id));
            //entity.update(new Design_Item().ItemName(meta.get("title"))
            //        .ItemDesc(meta.get("desc"))
            //        .Data(UMC.Data.JSON.serialize(meta))
            //        .ModifiedDate(new Date()).Seq(Utility.parse(meta.get("Seq"), 0)));


        }

        void Config(WebRequest request, Guid itemId)
        {


            UMC.Data.WebResource webr = UMC.Data.WebResource.Instance();

            var entity = Database.Instance().ObjectEntity<Design_Item>();
            entity.Where.Reset().And().Equal(new Design_Item() { Id = itemId });

            Design_Item item = entity.Single();
            switch (item.Type ?? 0)
            {
                case UIDesigner.StoreDesignTypeItem:
                case UIDesigner.StoreDesignTypeProduct:
                    item = entity.Where.Reset().And().Equal(new Design_Item() { Id = item.for_id }).Entities.Single();
                    break;
            }
            Design_Item finalItem = item;
            WebMeta meta = this.AsyncDialog(g =>
                    {



                        WebMeta data = UMC.Data.JSON.Deserialize<WebMeta>(finalItem.Data) ?? new UMC.Web.WebMeta();
                        //WebMeta data = Utility.isNull(UMC.Data.JSON.deserialize(finalItem.Data, WebMeta.class), new UMC.Web.WebMeta());

                        UIFormDialog from = new UIFormDialog();
                        from.Title = ("配置");


                        from.AddText("缩进", "Padding", data.Get("Padding") ?? "0");
                        from.AddNumber("展示顺序", "Seq", finalItem.Seq);


                        switch (finalItem.Type)
                        {
                            case UIDesigner.StoreDesignTypeBanners:
                                from.Title = ("广告横幅");
                                break;
                            case UIDesigner.StoreDesignTypeItems:
                                from.Title = ("分块区配置");
                                from.AddRadio("风格", "Model").Put("展示标题", "Title", data.Get("Model").Equals("Title") || data.ContainsKey("Model") == false).Put("仅显示图片 ", "Image", data.Get("Model").Equals("Image"));

                                break;
                            case UIDesigner.StoreDesignTypeTitleDesc:
                                from.Title = ("图文区配置");

                                String total = data.Get("Total") ?? "1";// data["Total"] ??"1";
                                String model = data.Get("Hide") ?? "";// data["Hide"] ??"";
                                ;
                                from.AddCheckBox("界面", "Hide", "T").Put("不显示标题", "HideTitle", model.IndexOf("HideTitle") > -1)
                                        .Put("不显描述 ", "HideDesc", model.IndexOf("HideDesc") > -1)
                                        .Put("不显左角价格 ", "HideLeft", model.IndexOf("HideLeft") > -1)
                                        .Put("不显右角说明 ", "HideRight", model.IndexOf("HideRight") > -1);


                                from.AddNumber("图文数量", "Total", Utility.Parse(total, 0));
                                break;
                            case UIDesigner.StoreDesignTypeCaption:

                                from.Title = "标题配置";
                                from.AddText("标题", "ItemName", item.ItemName);
                                from.AddCheckBox("标题隐藏", "Show", "Y").Put("隐藏", "Hide", data["Show"] == "Hide");

                                break;
                            case UIDesigner.StoreDesignTypeProducts:
                                from.Title = "商品展示配置";
                                from.AddText("标题", "ItemName", item.ItemName);
                                from.AddRadio("展示风格", "Model").Put("分块展示", "Area", data["Model"] == "Area" || data.ContainsKey("Model") == false).Put("分行展示 ", "Rows", data["Model"] == "Rows");

                                from.AddNumber("单行商品数", "Total", data["Total"] ?? "2");

                                break;
                            case UIDesigner.StoreDesignTypeCustom:
                                String config = data.Get("Config");
                                if (String.IsNullOrEmpty(config) == false && config.StartsWith("UI"))
                                {
                                    this.Context.Response.Redirect("Design", config);

                                }
                                else
                                {
                                    this.Prompt("参数错误");
                                }
                                break;
                            default:
                                this.Prompt("参数错误");
                                break;
                        }
                        from.Submit("确认", request, "Design");
                        return from;
                    }, "Setting");
            String show = meta.Get("Show");
            if (String.IsNullOrEmpty(show) == false)
            {
                meta.Put("Show", show.Contains("Hide") ? "Hide" : "Show");
            }
            entity.Where.Reset().And().Equal(new Design_Item() { Id = item.Id });
            entity.Update(new Design_Item
            {
                ItemName = meta["ItemName"],
                ModifiedDate = DateTime.Now,
                Seq = UMC.Data.Utility.IntParse(meta["Seq"], 0),
                Data = UMC.Data.JSON.Serialize(meta)
            });


        }

        void Banner(WebRequest request, Guid itemId)
        {

            UMC.Data.WebResource webr = UMC.Data.WebResource.Instance();

            var entity = Database.Instance().ObjectEntity<Design_Item>();
            entity.Where.Reset().And().Equal(new Design_Item() { Id = itemId });

            Design_Item item = entity.Single();

            WebMeta meta = this.AsyncDialog(g =>
                    {
                        switch (item.Type ?? 0)
                        {
                            case UIDesigner.StoreDesignTypeItem:
                                break;
                            case UIDesigner.StoreDesignTypeBanners:

                                Design_Item item2 = new Design_Item()
                                {
                                    Id = Guid.NewGuid(),
                                    design_id = item.design_id,
                                    for_id = item.Id,
                                    ModifiedDate = DateTime.Now,
                                    Type = UIDesigner.StoreDesignTypeItem
                                };

                                Design_Item max = entity
                              .Where.And().Equal(new Design_Item() { design_id = item.design_id, for_id = item.Id })
                              .Entities.Max(new Design_Item() { Seq = 0 });//.Seq+1;
                                item2.Seq = (max.Seq ?? 0) + 1;

                                entity.Insert(item2);



                                item = item2;
                                request.Arguments.Put("Id", item.Id);

                                break;
                            default:
                                this.Prompt("类型错误");
                                break;
                        }


                        var from = new UIFormDialog() { Title = "展示图片" };
                        var size = request.Arguments["Size"];
                        if (size == "none")
                        {
                            size = "默认尺寸100:55";
                        }
                        else
                        {

                            size = String.Format("参考尺寸:{0}", size);
                        }
                        from.AddFile(size, "_Image", webr.ResolveUrl(String.Format("{0}{1}/1/0.jpg!100", UMC.Data.WebResource.ImageResource, item.Id)))
                        .Command("Design", "Image", new UMC.Web.WebMeta().Put("id", item.Id).Put("seq", "1"));

                        from.AddNumber("展示顺序", "Seq", item.Seq);
                        from.Submit("确认", request, "Design");
                        return from;
                    }, "Setting");


            entity.Where.Reset().And().Equal(new Design_Item() { Id = item.Id });
            entity.Update(new Design_Item { Data = UMC.Data.JSON.Serialize(meta), ModifiedDate = DateTime.Now });


        }

        public override void ProcessActivity(WebRequest request, WebResponse response)
        {
            var user = UMC.Security.Identity.Current;
            String ssid = this.AsyncDialog("Id", d => this.DialogValue(user.Id.ToString()));
            Guid? sId = UMC.Data.Utility.Guid(ssid);
            String size = this.AsyncDialog("Size", g => this.DialogValue("none"));


            UMC.Data.WebResource webr = UMC.Data.WebResource.Instance();

            var entity = Database.Instance().ObjectEntity<Design_Item>();
            //entity.Where.Reset().And().Equal(new Design_Item() { Id = itemId });

            Design_Item item = null;//entity.single();

            if (sId.HasValue)
            {
                entity.Where.Reset().And().Equal(new Design_Item() { Id = sId });
                //entity.Where.And().Equal(new Design_Item().Id(sId));

                item = entity.Single();

            }

            if (item != null && item.Type != UIDesigner.StoreDesignType)
            {

                Design_Item finalItem = item;
                String type = this.AsyncDialog("Type", g =>
                            {

                                UIRadioDialog di = new UIRadioDialog();

                                switch (finalItem.Type)
                                {
                                    case UIDesigner.StoreDesignTypeCustom:
                                        break;
                                    case UIDesigner.StoreDesignType:
                                        di.Title = ("页面设计");
                                        di.Options.Put("编辑此项", "Edit");
                                        di.Options.Put("增加新项", "Append");
                                        di.Options.Put("删除此项", "Delete");
                                        break;
                                    case UIDesigner.StoreDesignTypeItem:
                                        di.Title = ("单项设计");
                                        di.Options.Put("编辑此项", "Edit");
                                        di.Options.Put("配置参数", "Config");
                                        di.Options.Put("增加新项", "Append");
                                        di.Options.Put("点击到...", "Union");
                                        di.Options.Put("删除此项", "Delete");
                                        break;
                                    case UIDesigner.StoreDesignTypeProduct:
                                        di.Title = ("商品栏位");
                                        di.Options.Put("调整顺序", "Seq");
                                        di.Options.Put("配置参数", "Config");
                                        di.Options.Put("增加商品", "Append");
                                        di.Options.Put("删除此项", "Delete");
                                        break;
                                    case UIDesigner.StoreDesignTypeDiscount:
                                        di.Title = ("卡券栏位");
                                        di.Options.Put("调整顺序", "Seq");
                                        //di.options().put("配置参数", "Config");
                                        di.Options.Put("增加卡券", "Append");
                                        di.Options.Put("删除此项", "Delete");
                                        break;
                                    case UIDesigner.StoreDesignTypeCaption:

                                        di.Title = ("栏目设计");
                                        di.Options.Put("编辑栏目", "Config");
                                        di.Options.Put("添加横幅区", "AddBanner");
                                        di.Options.Put("添加图标区", "AddIcon");
                                        di.Options.Put("添加分列区", "AddItem");
                                        di.Options.Put("添加图文区", "AddTitleDesc");

                                        di.Options.Put("删除栏目", "Delete");
                                        return di;
                                    case UIDesigner.StoreDesignTypeBanners:
                                        di.Title = ("横幅栏位");
                                        di.Options.Put("添加横幅页", "Banners");
                                        di.Options.Put("配置参数", "Config");
                                        di.Options.Put("删除横幅栏", "Delete");

                                        break;
                                    case UIDesigner.StoreDesignTypeProducts:
                                        di.Title = ("商品栏位");
                                        di.Options.Put("添加商品", "Product");
                                        di.Options.Put("配置参数", "Config");
                                        di.Options.Put("删除商品栏", "Delete");

                                        break;
                                    case UIDesigner.StoreDesignTypeDiscounts:
                                        di.Title = ("卡券栏位");
                                        di.Options.Put("添加卡券", "Discount");
                                        //di.options().put("配置参数", "Config");
                                        di.Options.Put("删除卡券栏", "Delete");

                                        break;
                                    case UIDesigner.StoreDesignTypeTitleDesc:
                                        di.Title = ("图文栏位");
                                        di.Options.Put("添加子项", "TitleDesc");

                                        di.Options.Put("配置图文", "Config");
                                        di.Options.Put("删除图文栏", "Delete");
                                        break;
                                    case UIDesigner.StoreDesignTypeItems:
                                        di.Title = ("分列栏位");
                                        di.Options.Put("添加子列", "Items");
                                        di.Options.Put("配置参数", "Config");
                                        di.Options.Put("删除分列栏", "Delete");
                                        break;
                                    case UIDesigner.StoreDesignTypeIcons:
                                        di.Title = ("图标栏位");
                                        di.Options.Put("添加子项", "Icons");
                                        di.Options.Put("配置参数", "Config");
                                        di.Options.Put("删除图标位", "Delete");
                                        break;
                                    default:
                                        break;

                                }
                                return di;
                            });
                switch (type)
                {
                    case "Seq":

                        Seq(request, response, item);
                        break;
                    case "Delete":
                        if (item.Type == UIDesigner.StoreDesignType)
                        {
                            entity.Where.Reset().And().Equal(new Design_Item() { design_id = item.Id });
                            if (entity.Count() > 0)
                            {
                                this.Prompt("请先删除子项");
                            }

                            entity.Where.Reset().And().Equal(new Design_Item() { design_id = sId });
                            entity.Delete();
                            this.Context.Send("Design", true);

                        }
                        else
                        {
                            entity.Where.Reset().And().Equal(new Design_Item() { for_id = item.Id });
                            if (entity.Count() > 0)
                            {
                                this.Prompt("请先删除子项");
                            }


                            entity.Where.Reset().And().Equal(new Design_Item() { design_id = sId });
                            entity.Delete();
                            this.Context.Send("Design", true);
                        }
                        break;
                    case "TitleDesc":
                        this.TitleDesc(request, sId.Value);
                        break;
                    case "Config":
                        this.Config(request, sId.Value);
                        break;
                    case "Union":
                        response.Redirect("Design", "Click", sId.ToString(), true);

                        break;
                    case "Icons":
                        Icons(request, sId.Value);
                        break;
                    case "Banners":
                        Banner(request, sId.Value);
                        break;
                    case "Items":
                        Items(request, sId.Value);
                        break;
                    case "Edit":
                        if (item.Type == UIDesigner.StoreDesignTypeCustom)
                        {
                            response.Redirect("Design", "Custom", new UMC.Web.WebMeta().Put("Id", item.Id.ToString(), "Size", size), true);
                        }
                        else
                        {
                            Design_Item eitem = entity.Where.Reset().And().Equal(new Design_Item() { Id = item.for_id }).Entities.Single();

                            switch (eitem.Type)
                            {
                                case UIDesigner.StoreDesignTypeTitleDesc:
                                    TitleDesc(request, sId.Value);
                                    break;
                                case UIDesigner.StoreDesignTypeBanners:
                                    Banner(request, sId.Value);
                                    break;
                                case UIDesigner.StoreDesignTypeIcons:
                                    Icons(request, sId.Value);
                                    break;
                                case UIDesigner.StoreDesignTypeItems:
                                    Items(request, sId.Value);
                                    break;

                            }
                        }
                        break;
                    case "AddCaption":
                        this.Caption(request, response, item.design_id.Value, item.Id.Value);
                        break;
                    case "AddTitleDesc":
                    case "AddProduct":
                    case "AddItem":
                    case "AddIcon":
                    case "AddBanner":


                        Design_Item item3 = new Design_Item() { Id = Guid.NewGuid(), for_id = item.Id, design_id = item.design_id };
                        //Design_Item item3 = new Design_Item().Id(Guid.randomGuid()).Design_id(item.design_id)
                        //        .For_id(item.Id);

                        switch (type)
                        {
                            case "AddProduct":
                                item3.Type = UIDesigner.StoreDesignTypeProducts;
                                break;
                            case "AddIcon":
                                item3.Type = UIDesigner.StoreDesignTypeIcons;
                                break;
                            case "AddTitleDesc":
                                item3.Type = UIDesigner.StoreDesignTypeTitleDesc;
                                break;
                            case "AddBanner":
                                item3.Type = UIDesigner.StoreDesignTypeBanners;
                                break;
                            case "AddItem":
                                item3.Type = UIDesigner.StoreDesignTypeItems;
                                break;
                            case "AddDiscount":
                                item3.Type = UIDesigner.StoreDesignTypeDiscounts;
                                break;


                        }

                        if (item3.Type != null)
                        {

                            Design_Item max = entity
                                    .Where.Reset().And().Equal(new Design_Item() { for_id = item.Id })
                                    .Entities.Max(new Design_Item() { Seq = 0 });//.Seq+1;
                            item3.Seq = (max.Seq ?? 0) + 1;


                            entity.Insert(item3);

                        }
                        break;
                    case "Append":

                        if (item.Type == UIDesigner.StoreDesignTypeCustom)
                        {
                            WebMeta meta = UMC.Data.JSON.Deserialize<WebMeta>(item.Data);
                            response.Redirect("Design", "Custom", new UMC.Web.WebMeta().Put("Config", meta.Get("Config")).Put("Size", size), true);
                        }
                        Design_Item aitem = entity.Where.Reset().And().Equal(new Design_Item() { Id = item.for_id }).Entities.Single();


                        switch (aitem.Type)
                        {
                            case UIDesigner.StoreDesignTypeTitleDesc:
                                TitleDesc(request, aitem.Id.Value);
                                break;
                            case UIDesigner.StoreDesignTypeBanners:
                                Banner(request, aitem.Id.Value);
                                break;
                            case UIDesigner.StoreDesignTypeIcons:
                                Icons(request, aitem.Id.Value);
                                break;
                            case UIDesigner.StoreDesignTypeItems:
                                Items(request, aitem.Id.Value);
                                break;
                        }

                        break;
                }
            }
            else
            {
                String type = this.AsyncDialog("Type", g =>
                {

                    UIRadioDialog di = new UIRadioDialog();
                    di.Title = ("界面设计");
                    di.Options.Put("添加标题栏", "Caption");
                    di.Options.Put("添加广告栏", "Banner");
                    di.Options.Put("添加图标栏", "Icons");
                    di.Options.Put("添加分块栏", "Items");
                    return di;
                });
                Design_Item item2 = new Design_Item() { Id = Guid.NewGuid(), for_id = Guid.Empty, design_id = sId };

                //Id=GU.nId(Guid.randomGuid()).Design_id(sId)
                //.For_id(Utility.uuidEmpty);


                switch (type)
                {
                    case "Caption":
                        Caption(request, response, sId.Value, Guid.Empty);
                        break;
                    case "TitleDesc":
                        item2.Type = UIDesigner.StoreDesignTypeTitleDesc;
                        break;
                    case "Products":
                        item2.Type = UIDesigner.StoreDesignTypeProducts;
                        break;
                    case "Icons":
                        item2.Type = UIDesigner.StoreDesignTypeIcons;
                        break;
                    case "Banner":
                        item2.Type = UIDesigner.StoreDesignTypeBanners;
                        break;
                    case "Items":
                        item2.Type = UIDesigner.StoreDesignTypeItems;

                        break;
                    case "Discounts":
                        item2.Type = UIDesigner.StoreDesignTypeDiscounts;

                        break;


                }
                if (item2.Type != null)
                {
                    Design_Item max = entity
                            .Where.Reset().And().Equal(new Design_Item { design_id = sId, for_id = Guid.Empty })
                            .Entities.Max(new Design_Item { Seq = 0 });//.Seq+1;
                    item2.Seq = (max.Seq ?? 0) + 1;


                    entity.Insert(item2);
                }


            }

            this.Context.Send("Design", true);

        }

    }
}