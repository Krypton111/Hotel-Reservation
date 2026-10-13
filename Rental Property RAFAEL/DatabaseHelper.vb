Imports System.Configuration
Imports Microsoft.Data.SqlClient

Public Module DatabaseHelper

    Private Const ConnectionStringName As String =
        "RentalPropertyDB"

    Public Function GetConnection() As SqlConnection

        Dim connectionSettings =
            ConfigurationManager.ConnectionStrings(
                ConnectionStringName
            )

        If connectionSettings Is Nothing Then

            Throw New Exception(
                "The connection string '" &
                ConnectionStringName &
                "' was not found in App.config."
            )

        End If

        Return New SqlConnection(
            connectionSettings.ConnectionString
        )

    End Function

End Module