provider "aws" {
  # region = "ap-southeast-2"
  # access_key = ""
  # secret_key = ""
}

module "s3" {
  source= "./modules/s3"
}


# module "network" {
#   source= "./modules/network"
# }

module "app" {
  source= "./modules/app"
  stage = var.stage
  deploy_bucket = module.s3.deploy_bucket
  code_version = var.code_version
  signature_certificate = var.signature_certificate
  signature_certiticate_password = var.signature_certiticate_password
  default_api_key = var.default_api_key
}
