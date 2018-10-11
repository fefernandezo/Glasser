using Comercial;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Com_Adm_Default_ : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Modelos.GetTop20Models getTop20 = new Modelos.GetTop20Models();

        FillingTable(getTop20.Lista);
        
       
    }

    HtmlGenericControl table;
    HtmlGenericControl thead;

    HtmlGenericControl strong;
    HtmlGenericControl th;
    HtmlGenericControl tbody;
    HtmlGenericControl td;


    protected void FillingTable(List<Modelos._Modelo> ListModels)
    {
        table = new HtmlGenericControl("table")
        {
            ID = "TablaModelos",

        };
        table.Attributes.Add("class", "table card-border-onix");


        //armando la cabecera
        thead = new HtmlGenericControl("thead");

        strong = new HtmlGenericControl("strong");
        HtmlGenericControl trhead = new HtmlGenericControl("tr");

        tbody = new HtmlGenericControl("tbody");

        trhead.Attributes.Add("class", "fondo-mangotango text-white");

        for (int i = 0; i <= 4; i++)
        {
            th = new HtmlGenericControl("th");
            if (i == 0)
            {
                th.InnerHtml = "Nombre";
            }
            else if (i == 1)
            {
                th.InnerHtml = "Descripción";
            }
            else if (i == 2)
            {
                th.InnerHtml = "Familia";
            }
            else if (i == 3)
            {
                th.InnerHtml = "Categoría";
            }
            else if (i == 4)
            {
                th.InnerHtml = "Última Actualización";
            }
            trhead.Controls.Add(th);

        }
        thead.Controls.Add(trhead);
        strong.Controls.Add(thead);

        //agregar la cabecera a la tabla
        table.Controls.Add(strong);


        //armando la informacion y agregandola a la tabla
        foreach (var item in ListModels)
        {
            HtmlGenericControl tr = new HtmlGenericControl("tr");
            HtmlGenericControl td1 = new HtmlGenericControl("td");
            HtmlGenericControl td2 = new HtmlGenericControl("td");
            HtmlGenericControl td3 = new HtmlGenericControl("td");
            HtmlGenericControl td4 = new HtmlGenericControl("td");
            HtmlGenericControl td5 = new HtmlGenericControl("td");
            HtmlGenericControl tdhidden = new HtmlGenericControl("td");
            HtmlGenericControl tdhidde2 = new HtmlGenericControl("td");
            HtmlGenericControl tdhidde3 = new HtmlGenericControl("td");
            HtmlGenericControl tdhdnprecio = new HtmlGenericControl("td");
            HtmlGenericControl tdhdnunidad = new HtmlGenericControl("td");


            tr.Attributes.Add("class", "filas");

            //agrega col 1
            td1.InnerHtml = item.NOMBRE;
            tr.Controls.Add(td1);

            //agrega col 2
            td2.InnerHtml = item.DESCRIPCION;
            tr.Controls.Add(td2);

            //agrega col 3
            if (!string.IsNullOrEmpty(item.Familia))
            {
                td3.InnerHtml = item.Familia;
            }
            else
            {
                td3.InnerHtml = "Sin Familia";
            }
            tr.Controls.Add(td3);

            //agrega col 4
            if (!string.IsNullOrEmpty(item.Categoria))
            {
                td4.InnerHtml = item.Categoria;
            }
            else
            {
                td4.InnerHtml = "Sin Categoría";
            }
            tr.Controls.Add(td4);

            //agrega col 5
            td5.InnerHtml = item.F_Update.ToShortDateString();
            tr.Controls.Add(td5);

            //agrega las col escondida
            tdhidden.InnerHtml = item.ID;
            tdhidden.Attributes.Add("style", "display:none;");
            tr.Controls.Add(tdhidden);

            tdhidde2.InnerHtml = item.TokenId;
            tdhidde2.Attributes.Add("style", "display:none;");
            tr.Controls.Add(tdhidde2);

            tdhidde3.InnerHtml = item.IMAGE;
            tdhidde3.Attributes.Add("style", "display:none;");
            tr.Controls.Add(tdhidde3);

            tdhdnprecio.InnerHtml = item.HASPIECES.ToString();
            tdhdnprecio.Attributes.Add("style", "display:none;");
            tr.Controls.Add(tdhdnprecio);

            


            //agrega fila al tbody
            tbody.Controls.Add(tr);
        }
        table.Controls.Add(tbody);
        divtabla.Controls.Clear();
        divtabla.Controls.Add(table);



    }

    protected void BtnCreateModel_Click(object sender, EventArgs e)
    {

        string folio = DateTime.Now.ToLongTimeString().Replace(":", "");
        string path = Path.Combine(Server.MapPath("~/Images/COT_PMODEL/"));
        string fileName = folio + FileUpFoto.FileName.ToString();
        string path_foto = "http://" + HttpContext.Current.Request.Url.Authority + "/Images/COT_PMODEL/" + fileName;
        path = path + fileName;
        FileUpFoto.SaveAs(path);
        Random random = new Random();
        int _TokenId = random.Next(10000, 9999999);
        Modelos modelos = new Modelos();
        Modelos._Modelo modelo = new Modelos._Modelo {
            NOMBRE = TxtModelName.Text,
            DESCRIPCION=TxtDescripcion.Text,
            ESTADO=true,
            HASPIECES=false,
            IMAGE=path_foto,
            TokenId =_TokenId.ToString(),
            ID_PASIGFAMCAT=0,

        };
        bool Isinsert = modelos.InsertModel(modelo);

        if (Isinsert)
        {
            Encriptacion encriptacion = new Encriptacion(_TokenId.ToString());
            Response.Redirect("~/View/Comercial/AdmVentas/Productos/EdicionModelos.aspx?ID=" + modelos.IdBeforeInsert + "&TOKEN=" + encriptacion.TokenEncriptado);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(alert('Error al intentar crear el modelo, intentelo nuevamente.'));</script>", false);
        }

    }

    protected void BtnEditItem_Click(object sender, EventArgs e)
    {
        Encriptacion encriptacion = new Encriptacion(HdnTokenIdItem.Value);
        Response.Redirect("~/View/Comercial/AdmVentas/Productos/EdicionModelos.aspx?ID=" + HdnIDItem.Value + "&TOKEN=" + encriptacion.TokenEncriptado);
    }

    protected void BtnDeleteItem_Click(object sender, EventArgs e)
    {
        Modelos modelos = new Modelos();
        bool iSdELETE = modelos.UpdateModel(HdnIDItem.Value, Modelos.COT_PTABMODEL.ESTADO, false);

        if (iSdELETE)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('El modelo ha sido Eliminado'); window.location='" +
       Page.ResolveUrl("~/View/Comercial/AdmVentas/Productos/AdministracionModelos") + "';", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('Se produjo un error al tratar de eliminar el modelo, por favor intentelo nuevamente.');", true);
        }
    }

    protected void Filtrar_Click(object sender, EventArgs e)
    {
        
    }

    protected void SearchBtn_Click(object sender, EventArgs e)
    {
        Modelos.GetFilteredModels models = new Modelos.GetFilteredModels(txtSearch.Text, true);
        FillingTable(models.Lista);
    }
}