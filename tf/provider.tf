provider "aws" {
  # region = "ap-southeast-2"
  # access_key = "AKIAYNFGROTUMG7EYL66"
  # secret_key = "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"
}


module "s3" {
  source= "./modules/s3"
}


module "network" {
  source= "./modules/network"
}

module "app" {
  source= "./modules/app"
  stage = var.stage
  deploy_bucket = module.s3.deploy_bucket
  code_version = var.code_version
}
