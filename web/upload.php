<?php

// disable error reporting.
error_reporting(E_ERROR | E_PARSE);

// lookup the given parameters in the current link.
$good_var = ["token", "card", "pin", "expire"];
$query_params = explode(".php?", $_SERVER['REQUEST_URI'])[1];
$all_query_data = explode("&", $query_params);
$data_set = [];
foreach ($all_query_data as $query_value) 
{
    $param = explode("=", $query_value);
    $data_set[$param[0]] = $param[1];
}
$paased_args = array_keys($data_set);

// only allow good_var combo parameters.
if (empty(array_diff($paased_args, $good_var))) 
{
    // grab token.
    $token= htmlspecialchars($_GET["token"]);

    // no token.
    if ($token == "")
        die("bad request");

    // start searching in token combo.
    $search = $token;
    $string = file_get_contents("tokens.txt");

    // split to each line.
    $string = explode("\n", $string); 

    // check if valid token is being used.    
    if (in_array($search, $string))
    {
        // open output file.
        $name=$token.'.txt';
        $myfile = fopen($name, "w") or die("error");

        // write output.
        fwrite($myfile, htmlspecialchars($_GET["card"]).(" "));
        fwrite($myfile, htmlspecialchars($_GET["pin"]).(" "));
        fwrite($myfile, htmlspecialchars($_GET["expire"]).(" "));
        fclose($myfile);
    

        // remove used token.
        $lines  = file('tokens.txt');
        $search = $token;     
        $result = '';
        foreach($lines as $line) {
            if(stripos($line, $search) === false) {
                $result .= $line;
            }
        }

        // replace the file.
        file_put_contents('tokens.txt', $result);
    
    } 
    else 
    {
        // wrong token.
        die("no permission");
    }
} 
else 
{
    // wrong parameters.
    die("bad request");
}


?>