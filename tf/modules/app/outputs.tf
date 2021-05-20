
output "base_url" {
  value = aws_api_gateway_deployment.rest_apideploy.invoke_url
}
