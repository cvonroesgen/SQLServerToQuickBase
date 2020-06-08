Imports System.Net
Imports System.Net.Http
Imports System.Web
Imports System.Web.Http
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Odbc

Public Class ValuesController
    Inherits ApiController

    ' GET api/values
    Public Function GetValues() As IEnumerable(Of String)
        Return New String() {"value1", "value2"}
    End Function

    ' GET api/values/25
    Public Function GetValue(ByVal id As Integer) As HttpResponseMessage
        Dim localConnection As SqlConnection
        Dim queryCommand As SqlCommand
        Dim insertCommand As OdbcCommand
        Dim MyDataReader As SqlDataReader
        Try
            'Create a Connection object.
            localConnection = New SqlConnection("server=.\SQLEXPRESS;database=qunect;Trusted_Connection=yes")

            'Create a Command object, and then set the connection.
            queryCommand = New SqlCommand("Select Image_Path, Image_ID from Images where Patient_ID = " & id, localConnection)

            With queryCommand
                'Set the command type that you will run.
                .CommandType = CommandType.Text
                'Open the connection.
                .Connection.Open()
                'Run the SQL statement, and then get the returned rows to the DataReader.
                MyDataReader = .ExecuteReader()
                Dim QuickBaseConnection As OdbcConnection = New OdbcConnection("DSN=demo.quickbase.com;")
                insertCommand = New OdbcCommand("INSERT INTO bqhf6s6wp (Image, Image_ID) VALUES (?, ?, ?)", QuickBaseConnection)
                insertCommand.Connection.Open()
                Dim insertTransaction As OdbcTransaction = QuickBaseConnection.BeginTransaction()
                insertCommand.Transaction = insertTransaction
                While MyDataReader.Read()
                    Dim imagePath As String = MyDataReader.GetString(0)
                    Dim imageID As Double = MyDataReader.GetDouble(1)
                    With insertCommand
                        .Parameters.Add("@Image", OdbcType.Char)
                        .Parameters("@Image").Value = imagePath
                        .Parameters.Add("@Image_ID", OdbcType.Double)
                        .Parameters("@Image_ID").Value = imageID
                .Parameters.Add("@Patient_ID", OdbcType.Double)
                .Parameters("@Patient_ID").Value = id
                        .CommandType = CommandType.Text
                        .ExecuteNonQuery()
                        .Parameters.Clear()
                    End With
                End While
                insertTransaction.Commit()
                insertCommand.Dispose()
                QuickBaseConnection.Close()
                .Dispose()  'Dispose of the Command object.
                localConnection.Close() 'Close the connection.
            End With
        Catch ex As Exception
            Dim errResponse As HttpResponseMessage = Request.CreateResponse(HttpStatusCode.OK)
            errResponse.Content = New StringContent(ex.Message)
            Return errResponse
        End Try
        Dim response As HttpResponseMessage = Request.CreateResponse(HttpStatusCode.Moved)
        response.Headers.Location = New Uri("https://demo.quickbase.com/db/bqhf749b7?a=dr&key=" & id & "&refresh=" & DateTime.Now.Ticks)
        Return response
    End Function

    ' POST api/values
    Public Sub PostValue(<FromBody()> ByVal value As String)

    End Sub

    ' PUT api/values/5
    Public Sub PutValue(ByVal id As Integer, <FromBody()> ByVal value As String)

    End Sub

    ' DELETE api/values/5
    Public Sub DeleteValue(ByVal id As Integer)

    End Sub
End Class
