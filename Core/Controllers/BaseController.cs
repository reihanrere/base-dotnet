namespace BaseDotnet.Core.Controllers;

using Microsoft.AspNetCore.Mvc;

public class BaseController: ControllerBase {
    protected IActionResult _Ok(Object data){
        return Ok(new {  message = "Success" , data = data });
    }

    protected IActionResult _Ok(Object data,String message){
        return Ok(new {  message = message , data = data });
    }

    protected IActionResult _Ok(Object data,int count){
        return Ok(new {  message = "Success" , data = data ,totalData = count});
    }
    protected IActionResult _BadRequest(Object _message){
        if(_message == null)
            _message = "Somethings Wrong, Please Contact Your Administrator";

        return BadRequest(new {  message = _message });
    }

    protected IActionResult _NotFound(Object _message){
        return NotFound(new {  message = _message });
    }
}