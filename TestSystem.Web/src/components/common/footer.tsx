import { Link } from "@nextui-org/link";

export const Footer = () => {
  return (
    <>
      <footer className="w-full flex items-center justify-center py-3">
        <Link
          isExternal
          className="flex items-center gap-1 text-current"
          href="https://www.williamto.com"
          title="nextui.org homepage"
        >
          <span className="text-default-600">Powered by</span>
          <p className="text-primary">www.williamto.com</p>
        </Link>
      </footer>
    </>
  );
};
