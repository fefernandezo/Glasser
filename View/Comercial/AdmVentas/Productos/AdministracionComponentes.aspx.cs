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
using Comercial;

public partial class Com_Adm_Comp_ : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {

            //cargar los 30 default
            _GetListComponentes _GetList = new _GetListComponentes();
            FillingTable(_GetList.ListComponentes);

        }
        
       
    }

    protected void BtnCrearComp_Click(object sender, EventArgs e)
    {
        string folio = DateTime.Now.ToLongTimeString().Replace(":", "");
        string path = Path.Combine(Server.MapPath("~/Images/COT_COMPONENTES/"));
        string fileName = folio + FileUpFoto.FileName.ToString();
        string path_foto = "http://" + HttpContext.Current.Request.Url.Authority + "/Images/COT_COMPONENTES/" + fileName;
        path = path + fileName;
        FileUpFoto.SaveAs(path);
        Random random = new Random();
        int TokenId = random.Next(10000, 9999999);
        ComponentesClass Componente = new ComponentesClass();
        bool Isinsert = Componente.InsertinTabla(TxtNombreComp.Text,TxtDescripcion.Text,path_foto,TokenId);

        if(Isinsert)
        {
            Encriptacion encriptacion = new Encriptacion(TokenId.ToString());
            Response.Redirect("~/View/Comercial/AdmVentas/Productos/EdicionComponente.aspx?ID=" + Componente.IdReturnedInsert + "&TOKEN=" + encriptacion.TokenEncriptado);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(alert('Error al intentar crear el componente, intentelo nuevamente.'));</script>", false);
        }
        

    }

    HtmlGenericControl table;
    HtmlGenericControl thead;
    
    HtmlGenericControl strong;
    HtmlGenericControl th;
    HtmlGenericControl tbody;
    HtmlGenericControl td;


    protected void FillingTable(List<_Componente> ListComponentes)
    {
        table = new HtmlGenericControl("table")
        {
            ID = "TablaComponentes",
            
        };
        table.Attributes.Add("class", "table card-border-onix");


        //armando la cabecera
        thead = new HtmlGenericControl("thead");

        strong = new HtmlGenericControl("strong");
        HtmlGenericControl trhead = new HtmlGenericControl("tr");

        tbody = new HtmlGenericControl("tbody");

        trhead.Attributes.Add("class", "card-bg-onix text-white");

        for (int i = 0; i <= 4; i++)
        {
            th = new HtmlGenericControl("th");
            if (i==0)
            {
                th.InnerHtml = "Nombre";
            }
            else if (i==1)
            {
                th.InnerHtml = "Descripción";
            }
            else if (i==2)
            {
                th.InnerHtml = "Precio";
            }
            else if (i==3)
            {
                th.InnerHtml = "Cod Alfak";
            }
            else if (i==4)
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
        foreach (var item in ListComponentes)
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
            td1.InnerHtml = item.Nombre;
            tr.Controls.Add(td1);

            //agrega col 2
            td2.InnerHtml = item.Descripcion;
            tr.Controls.Add(td2);

            //agrega col 3
            if (item.PrecioUn>0)
            {
                td3.InnerHtml = item.PrecioUn.ToString("C0", CultureInfo.CurrentCulture) + "/" + item.UnMedSimbolo;
            }
            else
            {
                td3.InnerHtml = "Sin Precio";
            }
            tr.Controls.Add(td3);

            //agrega col 4
            if (!string.IsNullOrEmpty(item.COD_ALFAK))
            {
                td4.InnerHtml = item.COD_ALFAK;
            }
            else
            {
                td4.InnerHtml = "Sin Código";
            }
            tr.Controls.Add(td4);

            //agrega col 5
            td5.InnerHtml = item.F_Actualizacion.ToShortDateString();
            tr.Controls.Add(td5);

            //agrega las col escondida
            tdhidden.InnerHtml = item._ID;
            tdhidden.Attributes.Add("style","display:none;");
            tr.Controls.Add(tdhidden);

            tdhidde2.InnerHtml = item.TokeId;
            tdhidde2.Attributes.Add("style", "display:none;");
            tr.Controls.Add(tdhidde2);

            tdhidde3.InnerHtml = item.Path_Photo;
            tdhidde3.Attributes.Add("style", "display:none;");
            tr.Controls.Add(tdhidde3);

            tdhdnprecio.InnerHtml = item.PrecioUn.ToString();
            tdhdnprecio.Attributes.Add("style", "display:none;");
            tr.Controls.Add(tdhdnprecio);

            tdhdnunidad.InnerHtml = item.UnidadMedida;
            tdhdnunidad.Attributes.Add("style", "display:none;");
            tr.Controls.Add(tdhdnunidad);


            //agrega fila al tbody
            tbody.Controls.Add(tr);
        }
        table.Controls.Add(tbody);
        divtabla.Controls.Clear();
        divtabla.Controls.Add(table);
        


    }

    protected void BtnEditItem_Click(object sender, EventArgs e)
    {
        Encriptacion encriptacion = new Encriptacion(HdnTokenIdItem.Value);
        Response.Redirect("~/View/Comercial/AdmVentas/Productos/EdicionComponente.aspx?ID=" + HdnIDItem.Value + "&TOKEN=" + encriptacion.TokenEncriptado);
    }

   

    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        ComponentesClass Comp = new ComponentesClass();
        bool IsDelete = Comp.UpdateComponente(HdnIDItem.Value, COT_MCOMPONENTES.Campos.ESTADO(), false);
        if (IsDelete)
        {
            
            Response.Redirect("~/View/Comercial/AdmVentas/Productos/AdministracionComponentes.aspx");
            
        }
    }

    protected void Filtrar_Click(object sender, EventArgs e)
    {

    }

    protected void SearchBtn_Click(object sender, EventArgs e)
    {
        _GetListComponentes GetC = new _GetListComponentes(txtSearch.Text);
        FillingTable(GetC.ListComponentes);
    }
}