import azure.functions as func
import json

def main(req: func.HttpRequest) -> func.HttpResponse:
    name = req.params.get('name')
    if not name:
        try:
            req_body = req.get_json()
        except ValueError:
            pass
        else:
            name = req_body.get('name')

    if name:
        return func.HttpResponse(
            json.dumps({"message": f"Hello, {name}!"}),
            status_code=200,
            mimetype="application/json"
        )
    else:
        return func.HttpResponse(
            json.dumps({"message": "Please provide a name parameter"}),
            status_code=400,
            mimetype="application/json"
        )