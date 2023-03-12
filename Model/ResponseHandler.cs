namespace Contact_Manager.Model
{
    public class ResponseHandler
    {
        public static ApiResponse GetExceptionResponse(Exception ex)
        {
            ApiResponse response = new ApiResponse();
            response.Code = "1";
            response.Message = ex.Message;
            return response;
        }

        public static ApiResponse GetAppResponse(ResponseType type, object? data)
        {
            ApiResponse response = new ApiResponse();
            response.ResponseData = data;

            switch (type)
            {
                case ResponseType.Success:
                    response.Code = "0";
                    response.Message = "Success";
                    break;

                case ResponseType.NotFound:
                    response.Code = "2";
                    response.Message = "No record available";
                    break;

                case ResponseType.Error:
                    response.Code = "3";
                    response.Message = "Error occurred";
                    break;
            }

            return response;
        }
    }
}

