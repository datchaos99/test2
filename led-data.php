<!DOCTYPE html>
<html>
<head>
	<style>
	table, th, td {
	  border: 1px solid black;
	}
	</style>
</head>

<body>

<?php

    /*
      Rui Santos
      Complete project details at https://RandomNerdTutorials.com/esp32-esp8266-mysql-database-php/
      
      Permission is hereby granted, free of charge, to any person obtaining a copy
      of this software and associated documentation files.
      
      The above copyright notice and this permission notice shall be included in all
      copies or substantial portions of the Software.
    */

    $servername = "sql100.epizy.com";
    $dbname = "epiz_28643236_device_control";
    $username = "epiz_28643236";
    $password = "ltncbtl";
    $table = "led";

    // Create connection
    $conn = new mysqli($servername, $username, $password, $dbname);
    // Check connection
    if ($conn->connect_error) {
        die("Connection failed: " . $conn->connect_error);
    } 

    $sql = "SELECT id, state FROM $table ORDER BY id ASC";

    echo '<table  cellspacing="5" cellpadding="2">
          <tr> 
            <td>ID</td> 
            <td>State</td> 
          </tr>';
     
    if ($result = $conn->query($sql)) {
        while ($row = $result->fetch_assoc()) {
            $row_id = $row["id"];
            $row_state = $row["state"];

            echo '<tr> 
                    <td>' . $row_id . '</td> 
                    <td>' . $row_state . '</td> 
                  </tr>';
        }
        $result->free();
    }

    $conn->close();
?> 
</table>
</body>
</html>
